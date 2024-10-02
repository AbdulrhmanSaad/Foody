function openModal(orderId) {
    // Set the hidden field to the order ID
    document.getElementById('orderId').value = orderId;
}

function submitStatusForm() {
    var form = document.getElementById('updateStatusForm');

    // Submit the form via AJAX
    $.ajax({
        url: form.action,
        type: 'POST',
        data: $(form).serialize(),
        success: function (response) {
            //toastr.success("Order Status updated succssfully!");
            $('#updateStatusModal').modal('hide');
            location.reload(); // Reload the page to show updated status
        },
        error: function () {
            alert('Failed to update order status.');
        }
    });
}

