(function () {
    var searchList = {};
    var indexCounter = 0;

    function addProductToCheckout(searchIndex, amount) {
        if (amount < 1) {
            alert("Ange ett antal större än 0!");
            return;
        }

        var prod = searchList[searchIndex];
        prod.Antal = amount;

        var exists = productExists(prod.Artikelnummer);

        if (exists) {
            addExisting(prod);
        }
        else {
            addNew(prod);
        }
        updateShoppingCart();
    };

    function addNew(prod) {
        addProductToCheckoutForm(prod, indexCounter);

        $('#checkout-list').append
            (
                '<ul id="checkout-product-id-' + prod.Artikelnummer + '" class="checkout-flex-container list-group">' +
                    '<li class="checkout-product-name checkout-flex-item list-group-item justify-content-between clearfix">' +
                        prod.Produktnamn +
                    '</li>' +
                    '<li class="checkout-flex-item list-group-item justify-content-between clearfix">' +
                        '<input class="form-control checkout-input" type="number" min="1" max="99" value="' + prod.Antal + '" />' +
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
    }

    function addExisting(prod) {
        var id = $("#checkout-product-id-" + prod.Artikelnummer + " button").attr("id")
        var index = id.charAt(id.length - 1);

        var currentValue = parseInt($("#checkout-product-id-" + prod.Artikelnummer).find("input").val());
        var currentFormValue = parseInt($("#checkout-form [name='products[" + index + "].Antal']").val());

        $("#checkout-product-id-" + prod.Artikelnummer).find("input").val(currentValue + parseInt(prod.Antal));

        $("#checkout-form [name='products[" + index + "].Antal']").val(currentFormValue + parseInt(prod.Antal));
    }

    function productExists(artnummer) {
        var exist = false;

        $("li ul").each(function () {
            var id = this.id.substring(this.id.length - 4, this.id.length);
            if (artnummer == id) {
                exist = true;
            }
        });
        return exist;
    }

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
                        '<li class="list-group-item justify-content-between flex-item clearfix">' +
                            '<img class="product-image img-responsive" src="' + searchList[i].BildURL + '" />' +
                            '<div class="pull-right mobile-version">' +
                                '<p class="product-name text-right">' + searchList[i].Produktnamn + '</p>' +
                                '<p class="product-price text-right text-primary dp-block">' + searchList[i].Pris + '<span>:-</span></p>' +
                                '<p class="text-right text-muted small">jmf.pris: <span class="product-jmf">' + searchList[i].Jmf + '</span>:-/kg</p>' +
                            '</div>' +
                        '</li>' +
                        '<li class="product-name list-group-item justify-content-between flex-item clearfix">' + searchList[i].Produktnamn + '</li>' +
                        '<li class="list-group-item justify-content-between flex-item clearfix">' + 
                            '<img class="product-company-image img-responsive" src="' + searchList[i].BildURL + '" />' +
                        '</li>' +
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
            addProductToCheckout(index, amount);
            if ($("ul .dropdown").hasClass("disabled")) {
                $("ul .dropdown").removeClass("disabled");
                $("ul .dropdown > a").removeClass("disabled");
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

    $("#checkout-list").on('click', '.checkout-btn-remove', function (e) {
        var index = this.id.charAt(this.id.length - 1);
        var id = $(this).closest("ul").attr("id");
        var artNumber = id.substring(id.length - 4, id.length);
        var amount = $("#checkout-product-id-" + artNumber).find("input").val();
        indexCounter--;
        $(".hidden-product-index-" + index).remove();
        $(this).closest("ul").remove();
        updateShoppingCart();
        
    });

    $("#checkout-list").on('change', '.checkout-input', function (e) {
        var id = $(this).closest("ul").attr("id");

        var index = $("#checkout-product-id-" + (id.substring(id.length - 4, id.length)) + " button").attr("id");
        index = index.charAt(index.length-1);

        var currentFormValue = parseInt($("#checkout-form [name='products[" + index + "].Antal']").val());

        $("#checkout-form [name='products[" + index + "].Antal']").val(parseInt($(this).val()));

        updateShoppingCart(e);
    });

    function updateShoppingCart(event) {
        var countProducts = 0;
        $("#checkout-list ul").each(function () {
            var id = this.id.substring(this.id.length - 4, this.id.length);
            var inputValue = parseInt($(this).find("input").val());
            countProducts += inputValue;
        });
        $('#dropdown-item-count').text(countProducts);

        if (parseInt($('#dropdown-item-count').text()) < 1) {
            $("ul .dropdown").addClass("disabled");
            $("ul .dropdown > a").addClass("disabled");
        }
        else if (event != null) {
            event.stopPropagation();
        }
    }

})();



