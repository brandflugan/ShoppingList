(function () {
    var searchList = {};
    var indexCounter = 0;

    function addProductToCheckout(searchIndex, amount) {
        if (amount < 0) {
            alert("Ange ett antal större än 0!");
            return;
        }

        var prod = searchList[searchIndex];
        prod.Antal = amount;

        addProductToCheckoutForm(prod, indexCounter);

        $('#checkout-list').append
            (
                '<ul class="checkout-flex-container list-group">' +
                    '<li class="checkout-product-name checkout-flex-item list-group-item justify-content-between clearfix">' +
                        prod.Produktnamn +
                    '</li>' +
                    '<li class="checkout-flex-item list-group-item justify-content-between clearfix">' +
                        '<input class="form-control" type="number" min="1" max="99" value="' + prod.Antal + '" />' +
                        '<span class="text-right text-muted"> st</span>' +
                    '</li>' +
                    '<li class="checkout-flex-item list-group-item justify-content-between clearfix">' +
                        '<button id="checkout-btn-remove-id-' + indexCounter + '" type="button" class="checkout-btn-remove btn btn-default btn-no-padding">' +
                            '<small><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></small>' +
                        '</button>' +
                    '</li>' +
                '</ul>'
            );

        indexCounter++;
    };

    function addProductToCheckoutForm(product, index) {
        $("#checkout-form").append('<input class="hidden-product-index-' + index + '" name="products[' + index + '].Artikelnummer" value="' + product.Artikelnummer + '" type="hidden"/>');
        $("#checkout-form").append('<input class="hidden-product-index-' + index + '" name="products[' + index + '].Antal" value="' + product.Antal + '" type="hidden"/>');
    }

    function listProducts() {

        $('#product-search').html('');
        for (i = 0; i < searchList.length; i++) {
            $('#product-search').append
                (
                    '<ul class="list-group flex-container list-special">' +
                    ' <li class="list-group-item justify-content-between flex-item clearfix"> ' +
                        '<img class="product-image img-responsive" src="' + searchList[i].BildURL + '" />' +
                        '<div class="pull-right mobile-version">' +
                            '<p class="product-name text-right">' + searchList[i].Produktnamn + '</p>' +
                            '<p class="product-price text-right text-primary dp-block">' + searchList[i].Pris + '<span>:-</span></p>' +
                            '<p class="text-right text-muted small">jmf.pris: <span class="product-jmf">' + searchList[i].Jmf + '</span>:-/kg</p>' +
                        '</div>' +
                    '</li>' +
                    '<li class="product-name list-group-item justify-content-between flex-item clearfix">' + searchList[i].Produktnamn + '</li>' +
                    '<li class="list-group-item justify-content-between flex-item clearfix">' +
                        '<span class="product-price text-primary dp-block">' + searchList[i].Pris + ':-</span>' +
                        '<span class="product-jmf text-muted small">jmf.pris: <span class="product-jmf">' + searchList[i].Jmf + '</span>:-/kg</span>' +
                    '</li>' +
                    '<li class="list-group-item justify-content-between flex-item clearfix">' +
                        '<input id="product-count-id-' + i + '" class="product-count form-control" type="number" min="1" max="99" value="1" />' +
                        '<span class="text-right text-muted"> st</span>' +
                    '</li>' +
                    '<li class="list-group-item justify-content-between flex-item clearfix">' +
                        '<input id="product-id-' + i + '" class="product-btn-add btn btn-default" type="button" value="Lägg till" />' +
                    '</li>' +
                    '</ul>'
                )
        }

        $(".product-btn-add").on("click", function () {
            var index = this.id.charAt(this.id.length - 1);
            var amount = $('#product-count-id-' + index).val();
            $('#dropdown-item-count').text(parseInt($('#dropdown-item-count').text()) + parseInt(amount));
            addProductToCheckout(index, amount);
            if ($("ul .dropdown").hasClass("disabled")) {
                $("ul .dropdown").removeClass("disabled");
            }
        })
    };

    function searchProducts() {
        var searchvalue = $("#searchfield").val();

        $.ajax({
            url: "/matkris/search",
            type: "POST",
            data: { searchterm: searchvalue },
            success: function (toproducts) {
                searchList = toproducts;
                listProducts();
            }
        });
    };

    $("#searchfield").on("keyup", function (e) {
        if ($("#searchfield").val().length > 1) {
            searchProducts();
        }
    });

    $(document).on('click', '.checkout-btn-remove', function (e) {
        $(this).closest("ul").remove();
        $('#dropdown-item-count').text(parseInt($('#dropdown-item-count').text()) - 1);
        var index = this.id.charAt(this.id.length - 1);
        $(".hidden-product-index-" + index).remove();
        if (parseInt($('#dropdown-item-count').text()) < 1) {
            $("ul .dropdown").addClass("disabled");

        }
        else {
            e.stopPropagation();
        }
    });

})();



