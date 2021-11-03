// 側選單設定
$("#iconPromotion").hover(
    function () {
        $(this).addClass('sliderRed');
        $("#navPromotion").show();
    },
    function () {
        $(this).removeClass('sliderRed');
        $("#navPromotion").hide();
    }
);

$("#iconGift").hover(
    function () {
        $(this).addClass('sliderOrange');
        $("#navGift").show();
    },
    function () {
        $(this).removeClass('sliderOrange');
        $("#navGift").hide();
    }
    );

$("#iconBank").hover(
    function () {
        $(this).addClass('sliderYellow');
        $("#navBank").show();
    },
    function () {
        $(this).removeClass('sliderYellow');
        $("#navBank").hide();
    }
    );

$("#iconReport").hover(
    function () {
        $(this).addClass('sliderGreen');
        $("#navReport").show();
    },
    function () {
        $(this).removeClass('sliderGreen');
        $("#navReport").hide();
    }
    );

$("#iconUser").hover(
    function () {
        $(this).addClass('sliderBlue');
        $("#navUser").show();
    },
    function () {
        $(this).removeClass('sliderBlue');
        $("#navUser").hide();
    }
);

$("#iconMember").hover(
    function () {
        $(this).addClass('sliderBlue');
        $("#navUser").show();
    },
    function () {
        $(this).removeClass('sliderBlue');
        $("#navUser").hide();
    }
);

$("#iconAuth").hover(
    function () {
        $(this).addClass('sliderIndigo');
        $("#navAuth").show();
    },
    function () {
        $(this).removeClass('sliderIndigo');
        $("#navAuth").hide();
    }
    );

$("#iconSignoff").hover(
    function () {
        $(this).addClass('sliderPurple');
       $("#navSignoff").show();
    },
    function () {
        $(this).removeClass('sliderPurple');
        $("#navSignoff").hide();
    }
    );

$("#navPromotion").hover(
    function () {
        $(this).show();
        $("#iconPromotion").addClass('sliderRed');
    },
    function () {
        $(this).hide();
        $("#iconPromotion").removeClass('sliderRed');
    })

$("#navGift").hover(
    function () {
        $(this).show();
        $("#iconGift").addClass('sliderOrange');
    },
    function () {
        $(this).hide();
        $("#iconGift").removeClass('sliderOrange');
    })

$("#navBank").hover(
    function () {
        $(this).show();
        $("#iconBank").addClass('sliderYellow');
    },
    function () {
        $(this).hide();
        $("#iconBank").removeClass('sliderYellow');
    })

$("#navReport").hover(
    function () {
        $(this).show();
        $("#iconReport").addClass('sliderGreen');
    },
    function () {
        $(this).hide();
        $("#iconReport").removeClass('sliderGreen');
    })


$("#navUser").hover(
    function () {
        $(this).show();
        $("#iconUser").addClass('sliderBlue');
    },
    function () {
        $(this).hide();
        $("#iconUser").removeClass('sliderBlue');
    })

$("#navAuth").hover(
    function () {
        $(this).show();
        $("#iconAuth").addClass('sliderIndigo');
    },
    function () {
        $(this).hide();
        $("#iconAuth").removeClass('sliderIndigo');
    })

$("#navSignoff").hover(
    function () {
        $(this).show();
        $("#iconSignoff").addClass('sliderPurple');
    },
    function () {
        $(this).hide();
        $("#iconSignoff").removeClass('sliderPurple');
    })