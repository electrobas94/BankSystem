function setRequired(flag) {
    var elem_list = $(".req");

    for (var i = 0; i < elem_list.length; i++)
        elem_list[i].required = flag;
}