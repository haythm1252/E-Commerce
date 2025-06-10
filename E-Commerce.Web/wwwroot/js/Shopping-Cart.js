$(document).ready(function () {

    function updatePriceAndTotal() {
        let total = 0;
        $('.cart-item').each(function () {
            let quantity = parseInt($(this).find('.quantity-input').val());
            let unitPrice = parseFloat($(this).find('.price').data('unitprice'));
            let itemTotal = quantity * unitPrice;
            $(this).find('.price').text(itemTotal.toFixed(2) + ' EGP');
            total += itemTotal;
        });
        $('#cart-total').text('Total: ' + total.toFixed(2) + ' EGP');
    }

    $(".decrease-btn").click(function () {
        var btn = $(this);
        var quantityInput = btn.siblings('.quantity-input');
        var value = parseInt(quantityInput.val());
        var min = parseInt(quantityInput.attr("min"));
        var productId = quantityInput.data('id');

        if (value > min) {
            quantityInput.val(value - 1);
            $.ajax({
                url: 'ShoppingCart/RemoveFromCart/' + productId,
                type: 'POST',
                success: function () {
                    updatePriceAndTotal();
                },
                error: function () {
                    toastr.error('Failed to remove product from cart.', 'Error', {
                        timeOut: 3000,
                        positionClass: 'toast-bottom-right'
                    });
                }
            });
        }
        if (value == min) {
            $.ajax({
                url: 'ShoppingCart/RemoveFromCart/' + productId,
                type: 'POST',
                success: function () {
                    btn.closest('.cart-item').remove();
                    updatePriceAndTotal();
                },
                error: function () {
                    toastr.error('Failed to remove product from cart.', 'Error', {
                        timeOut: 3000,
                        positionClass: 'toast-bottom-right'
                    });
                }
            });
        }
    });

    $(".increase-btn").click(function () {
        var quantityInput = $(this).siblings('.quantity-input');
        var value = parseInt(quantityInput.val());
        var max = parseInt(quantityInput.attr("max"));
        var productId = quantityInput.data('id');

        if (value < max) {
            quantityInput.val(value + 1);
            $.ajax({
                url: 'ShoppingCart/AddToCart/' + productId,
                type: 'POST',
                success: function () {
                    updatePriceAndTotal();
                },
                error: function () {
                    toastr.error('Failed to add product to cart.', 'Error', {
                        timeOut: 3000,
                        positionClass: 'toast-bottom-right'
                    });
                },
            });
        }
    });

    $(".quantity-input").on("input", function () {
        var value = $(this).val().replace(/[^0-9]/g, '');
        var min = parseInt($(this).attr("min"));
        var max = parseInt($(this).attr("max"));

        if (value === '' || parseInt(value) < min) {
            value = min;
        } else if (parseInt(value) > max) {
            value = max;
        }

        $(this).val(value);
        updatePriceAndTotal();
    });
    updatePriceAndTotal();
});
