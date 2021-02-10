$(function () {
    var DeleteProduct =
        function () {
            var $form = $(this);
            var options = {
                url: $form.attr("action"),
                type: $form.attr("method"),
                data: {
                    'id': $form.attr("id"),
                    '__RequestVerificationToken': $("tr span[id=ProducttokenId_" + $(this)[0].id + "] input")[0].value
                }
            };
            $.ajax(options).done(function (data) {
                if (data != null || data != "") {
                    $("#Id_" + $(this)[0].data.split("=")[1].split("&")[0] + "").remove();
                    $(".SuccessResult").show();
                    setTimeout(function () {
                        $(".SuccessResult").hide();
                    }, 2000);

                }
            });
        };
    var AddToCart =
        function () {
            var $form = $(this);
            var options = {
                url: $form.attr("action"),
                type: $form.attr("method"),
                data: {
                    'id': $form.attr("id"),
                    '__RequestVerificationToken': $("tr span[id=CarttokenId_" + $(this)[0].id + "] input")[0].value
                }
            };
            $.ajax(options).done(function (data) {
                var itemInCartCount = 0;
                if (isNaN(parseInt($("#ItemsinCartCount")[0].innerText))) {
                    itemsInCartCount = 0;
                    $("#ItemsinCartCount")[0].innerText = 0;
                }
                else {
                    $("#ItemsinCartCount")[0].innerText = parseInt($("#ItemsinCartCount")[0].innerText) + 1;
                }
                if (data != null || data != "") {
                    $(".CartSuccessResult").show();
                    setTimeout(function () {
                        $(".CartSuccessResult").hide();
                    }, 2000);
                }
            });
        }
    var PlaceOrder =
        function () {
            var IDs = [];
            $("tr input[id = item_ProductId]").each(function () { IDs.push(this.value); });
            var $form = $(this);
            var options = {
                url: $form.attr("action"),
                type: $form.attr("method"),
                data: {
                    IDs: IDs,
                    '__RequestVerificationToken': $("span[id=tokenId_PlacedOrder] input")[0].value
                }
            };
            $.ajax(options).done(function (data) {
                if (data == "Success") {
                    $("#PlaceOrder").hide();
                    $("#gotohome").hide();
                    $(".OrderSuccessResult").show();
                    setTimeout(function () {
                        $(".OrderSuccessResult").hide();
                    }, 2000);
                    $("#HomeAndOrder").show();
                }
                else {
                    $(".OrderFailureResult").show();
                    setTimeout(function () {
                        $(".OrderFailureResult").hide();
                    }, 2000);
                }
            });
        }
    var RemoveItemFromCart =
        function () {
            var $form = $(this);
            var options = {
                url: $form.attr("action"),
                type: $form.attr("method"),
                data: {
                    'id': $form.attr("id"),
                    '__RequestVerificationToken': $("tr span[id=tokenId_" + $(this)[0].id + "] input")[0].value
                }
            };
            $.ajax(options).done(function (data) {
                if (data != null || data != "") {
                    var itemPrice = $("tr td[id=SalePrice_" + ($(this)[0].data.split("=")[1]).split("&")[0]+"]")[0].outerText;
                    $("#spantotalprice")[0].innerText = parseFloat($("#spantotalprice")[0].innerText) - itemPrice;
                    $("#spantotalproducts")[0].innerText = parseInt($("#spantotalproducts")[0].innerText) - 1;
                    $("#Id_" + ($(this)[0].data.split("=")[1]).split("&")[0]).remove();
                    $("#ItemsinCartCount")[0].innerText = parseInt($("#ItemsinCartCount")[0].innerText) - 1;

                    $(".CartSuccessResult").show();
                    setTimeout(function () {
                        $(".CartSuccessResult").hide();
                    }, 2000);

                }
            });
        };
    $("a[data-pmc-ajax='true']").click(AddToCart);
    $("a[data-pm-ajax='true']").click(DeleteProduct);
    $("a[data-pmco-ajax='true']").click(PlaceOrder);
    $("a[data-pmrci-ajax='true']").click(RemoveItemFromCart);

});
