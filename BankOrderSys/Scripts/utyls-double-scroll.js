﻿$(document).ready(function () {
    $('.double-scroll').doubleScroll();
});

(function ($) {
    jQuery.fn.doubleScroll = function (userOptions) {
        var options = {
            contentElement: undefined,
            scrollCss: {
                'overflow-x': 'auto',
                'overflow-y': 'hidden'
            },
            contentCss: {
                'overflow-x': 'auto',
                'overflow-y': 'hidden'
            },
            onlyIfScroll: true,
            resetOnWindowResize: false,
            timeToWaitForResize: 30
        };

        $.extend(true, options, userOptions);

        $.extend(options, {
            topScrollBarMarkup: '<div class="doubleScroll-scroll-wrapper" style="height: 20px;"><div class="doubleScroll-scroll" style="height: 20px;"></div></div>',
            topScrollBarWrapperSelector: '.doubleScroll-scroll-wrapper',
            topScrollBarInnerSelector: '.doubleScroll-scroll'
        });

        var _showScrollBar = function ($self, options) {

            if (options.onlyIfScroll && $self.get(0).scrollWidth <= $self.width()) {
                $self.prev(options.topScrollBarWrapperSelector).remove();
                return;
            }

            var $topScrollBar = $self.prev(options.topScrollBarWrapperSelector);

            if ($topScrollBar.length == 0) {
                $topScrollBar = $(options.topScrollBarMarkup);
                $self.before($topScrollBar);

                $topScrollBar.css(options.scrollCss);
                $self.css(options.contentCss);

                $topScrollBar.bind('scroll.doubleScroll', function () {
                    $self.scrollLeft($topScrollBar.scrollLeft());
                });

                var selfScrollHandler = function () {
                    $topScrollBar.scrollLeft($self.scrollLeft());
                };
                $self.bind('scroll.doubleScroll', selfScrollHandler);
            }

            var $contentElement;

            if (options.contentElement !== undefined && $self.find(options.contentElement).length !== 0) {
                $contentElement = $self.find(options.contentElement);
            } else {
                $contentElement = $self.find('>:first-child');
            }

            $(options.topScrollBarInnerSelector, $topScrollBar).width($contentElement.outerWidth());
            $topScrollBar.width($self.width());
            $topScrollBar.scrollLeft($self.scrollLeft());
        }

        return this.each(function () {
            var $self = $(this);

            _showScrollBar($self, options);

            if (options.resetOnWindowResize) {
                var id;
                var handler = function (e) {
                    _showScrollBar($self, options);
                };

                $(window).bind('resize.doubleScroll', function () {
                    clearTimeout(id);
                    id = setTimeout(handler, options.timeToWaitForResize);
                });
            }
        });
    }
}(jQuery));