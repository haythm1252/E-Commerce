$(document).ready(function () {
    $('.AddToCart').on('click', function (e) {
        e.preventDefault();
        var productId = $(this).data('id');
        $.ajax({
            url: '/ShoppingCart/AddToCart/' + productId,
            type: 'POST',
            success: function () {
                toastr.success('Product added to cart successfully!', 'Success', {
                    timeOut: 3000,
                    positionClass: 'toast-bottom-right'
                });
            },
            error: function () {
                toastr.error('Failed to add product to cart.', 'Error', {
                    timeOut: 3000,
                    positionClass: 'toast-bottom-right'
                });
            }
        });
    });
});