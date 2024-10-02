async function add(productId, qyt = 1) {

        console.log("quantity : "+qyt);
        $.ajax({
            url: "/Cart/AddItem",
            data: { "productId": productId,"qyt":qyt },
            success: function (result) {
                toastr.success("Item added to cart successfully!");
                console.log(result);
                var cartCount = document.getElementById("cartCount");
                cartCount.innerHTML = result;
                //     document.getElementById("stud").innerHTML = ""
                //     for (let stud of result)
                //         document.getElementById("stud").innerHTML += "<option value='" + stud.id + "'>" + stud.name + "</option>"
            },
            error: function () {
                toastr.error("An error occurred while adding the item to the cart.");
            }
        });
    }
