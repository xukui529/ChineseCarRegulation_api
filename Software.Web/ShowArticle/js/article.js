var fontFlag = 'middle';
var langFlag = "chaina";
var likeFlag = "nolike";
$(function () {
    console.log(GetQueryString("id"));
    var ly;
    var key = GetQueryString("id"); 
    var stypeDefult = 4;
    if (DocShowType == '1') {
        stypeDefult = 2;
    }
    if (DocShowType == '2') {
        stypeDefult = 3;
    }
    if (DocShowType == '3') {
        stypeDefult = 2;
    }
    if (DocShowType == '4') {
        stypeDefult = 4;
    }


    var stype = GetQueryString("type");
    key = key == null ? "bc4972fc-f442-4932-a66b-6417568e0981" : key;
    stype = stype == null ? stypeDefult : stype;
    GetDetail(stype);
    //加载详情
    function GetDetail(_type) {
        $.ajax({
            type: "POST",
            url: "/ContentManage/DocumentUpload/GetDocumentInfoById",
            data: "{ 'id':'" + key + "','type': " + _type + "}",
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            async: true,
            success: function (data) {
                layer.close(ly);
           //     console.log(data[0]);
                if (data[0] != '') {
                    $("#article").html(data[0]);
                }
            },
            beforeSend: function (XMLHttpRequest) {
                ly = layer.load(2, { shade: false }); //0代表加载的风格，支持0-2
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.close(ly);
            }
        });
    }

    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg);  //匹配目标参数
        if (r != null) return unescape(r[2]); return null; //返回参数值
    }
    $("#backTop").click(function () {
        $("html,body").animate({ scrollTop: 0 }, 500);
    });
    $("#fontSize button").click(function () {
        $("#fontSize button").removeClass("font-active");
        $(this).addClass("font-active")
        fontFlag = this.id;
    })
    $(".lan").click(function () {
        $(".lan").removeClass("font-active");
        $(this).addClass("font-active")
        langFlag = this.id;
    })

    $(".heng").click(function () {
        $(".heng").removeClass("active");
        $(".shu").removeClass("active");
        $(this).addClass("active")
    })
    $(".shu").click(function () {
        $(".heng").removeClass("active");
        $(".shu").removeClass("active");
        $(this).addClass("active")
    })
    $("#like").click(function () {
        if (likeFlag === "nolike") {
            $(".fav").removeClass("no-heart");
            $(".fav").addClass("heart");
            $(".wenzi").html("已收藏");
            likeFlag = "like";
        } else {
            $(".fav").removeClass("heart");
            $(".fav").addClass("no-heart");
            $(".wenzi").html("未收藏");
            likeFlag = "nolike";
        }
    })
    $(window).scroll(function (event) {
        if ($(window).scrollTop() > 0) {
            $(".line3").hide()
        } else {
            $(".line3").show();
        }
        // console.log($(window).scrollTop()>0)
    });
    $("#contents_list li").mouseover(function () {
        $(this).children("a").addClass("active_logo");
    })
    //set heng shu active 中china 英english  font-active
    $(".shu").click(function () {
        GetDetail(4);
        $(".heng").removeClass("active");
        $(".shu").addClass("active");
        $(".china").removeClass("font-active");
        $(".english").removeClass("font-active");
    });
    $(".heng").click(function () {
        GetDetail(1);
        $(".heng").addClass("active");
        $(".shu").removeClass("active");
        $(".china").removeClass("font-active");
        $(".english").removeClass("font-active");
    });
    $(".china").click(function () {        
        $(".heng").removeClass("active");
        $(".shu").removeClass("active");
        $(".china").addClass("font-active");
        $(".english").removeClass("font-active");
        GetDetail(2);
    });
    $(".english").click(function () {
        $(".heng").removeClass("active");
        $(".shu").removeClass("active");
        $(".china").removeClass("font-active");
        $(".english").addClass("font-active");
        GetDetail(3);
    });
    //set article fontsize
    $("#small").click(function () {
        setFongSize("12px");
        $("#middle").removeClass("font-active");
        $("#small").addClass("font-active");
        $("#big").removeClass("font-active");
    });
    $("#middle").click(function () {
        setFongSize("16px");
        $("#middle").addClass("font-active");
        $("#small").removeClass("font-active");
        $("#big").removeClass("font-active");
    });
    $("#big").click(function () {
        setFongSize("20px");
        $("#middle").removeClass("font-active");
        $("#small").removeClass("font-active");
        $("#big").addClass("font-active");
    });
    function setFongSize(s) {
        $("#article *").css("font-size", s);
    }
    //change skin
    function replacejscssfile(csshref) {
        let links = $('head').find("link[cssd=aabb]");
        links.attr('href', 'css/' + csshref);
    }
    //sun  article.css
    $("#sun").click(function () {
        $(".black").attr("class", "white");
        replacejscssfile("article.css");
    });
    //moon skinnight
    $("#moon").click(function () {
        $(".white").attr("class", "black");
        replacejscssfile("skinnight.css");
    });

})
