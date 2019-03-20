$(function ($) {
    $('#sidebar-nav').on('click', '.dropdown-toggle', function (e) {
        e.preventDefault();
        var $item = $(this).parent();
        if (!$item.hasClass('active')) {
            $item.parent().find('.active .nav nav-second-level collapse').slideUp('fast');
            $item.parent().find('.active').toggleClass('active');
        }
        $item.toggleClass('active');

        if ($item.hasClass('active')) {
            $('#sidebar-nav').find('ul').removeClass(' in');

            $item.find('ul').addClass(' in');
            $item.children('.nav nav-second-level collapse').slideDown('fast', function () {
                var _height1 = $(window).height() - 92 - $item.position().top;
                var _height2 = $item.find('ul.nav nav-second-level collapse').height() + 10;
                var _height3 = _height2 > _height1 ? _height1 : _height2;
                $item.find('ul.nav nav-second-level collapse').css({
                    overflow: "auto",
                    height: _height3
                })
            });
        }
        else {
            $item.find('ul').removeClass(' in');
            $item.children('.nav nav-second-level collapse').slideUp('fast');
        }
    });

    GetLoadNav()
});
function GetLoadNav() {
    var data = top.clients.authorizeMenu;
    var _html = "";
    var flag = true;
    $.each(data, function (i) {
        var row = data[i];
        if (row.F_ParentId == "0") {
            _html += '<li class="">';
            _html += '<a data-id="' + row.F_Id + '" href="javascript:;" class="dropdown-toggle"><i class="' + row.F_Icon + '"></i><span class="nav-label">' + row.F_FullName + '</span><span class="fa arrow"></span></a>';
            var childNodes = row.ChildNodes;
            if (childNodes.length > 0) {
                _html += '<ul class="nav nav-second-level collapse">';
                $.each(childNodes, function (i) {
                    var subrow = childNodes[i];
                    _html += '<li>';
                    _html += '<a class="" data-id="' + subrow.F_Id + '" href="' + subrow.F_UrlAddress + '" data-index="' + subrow.F_SortCode + '">' + subrow.F_FullName + '</a>';
                    _html += '</li>';
                });
                _html += '</ul>';
            }
            _html += '</li>';
        }
    });
    $("#sidebar-nav").append(_html);
}