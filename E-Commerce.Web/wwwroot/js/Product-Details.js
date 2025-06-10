
    // Additional JavaScript for better behavior
$(document).ready(function() {
    // Decrease quantity
    $(".quantity-btn:first-of-type").click(function () {
        var value = parseInt($("#quantity").val());
        var min = parseInt($("#quantity").attr("min"));
        if (value > min) {
            $("#quantity").val(value - 1);
        }
    });

// Increase quantity
$(".quantity-btn:last-of-type").click(function() {
            var value = parseInt($("#quantity").val());
var max = parseInt($("#quantity").attr("max"));
if (value < max) {
    $("#quantity").val(value + 1);
            }
        });

// Ensure only numbers are entered
$("#quantity").on("input", function() {
            var value = $(this).val().replace(/[^0-9]/g, '');
var min = parseInt($(this).attr("min"));
var max = parseInt($(this).attr("max"));

if (value === '' || parseInt(value) < min) {
    value = min;
            } else if (parseInt(value) > max) {
    value = max;
            }

$(this).val(value);
        });
    });
