/// <reference path="cartmanage.js" />
$(document).ready(function () {
        $('.nav-pills .nav-item a').click(function (event) {
            event.preventDefault(); // Prevent the default action (e.g., page reload)
            var category = $(this).data('category');
            console.log('Clicked category:', category);
            var targetContentId = $(this).attr('href');
            console.log('Clicked category:', targetContentId);
            $('#start').empty();
            $('#tab-1').empty();
            $('#tab-2').empty();
            $('#tab-3').empty();
            $(targetContentId).empty();

            console.log('Clicked category:', category);
            $.ajax({
                url: '/Product/GetProductsByCategory', // Replace with your controller and action
                type: 'GET',
                data: { categoryId: category },
                success: function (response) {
                    // Handle the response from the server
                    console.log('Server response:', response);
                    if (response.length > 0) {
                        for (let product of response) {
                            console.log(product);
                            var productHtml = `
                                                <div class="col-xl-3 col-lg-4 col-md-6 wow fadeInUp" data-wow-delay="0.1s">
                                                  <div  class="product-item">
                                                    <div class="position-relative bg-light overflow-hidden">
                                                                   <img class="img-fluid w-100" src="/img/Products/${product.imgeUrl}" alt="no image" style="height:258px">
                                                               <div class="bg-secondary rounded text-white position-absolute start-0 top-0 m-4 py-1 px-3">${product.quantity}</div>
                                                        </div>
                                                        <div class="text-center p-4">
                                                                    <a class="d-block h5 mb-2" href="/Product/Details/${product.id}">${product.name}</a>
                                                            <span class="text-body">${product.price} EGP</span>
                                                        </div>
                                                        <div class="d-flex border-top">
                                                            <small class="w-50 text-center border-end py-2">
                                                                <a class="text-body" href="/Product/Details/${product.id}"><i class="fa fa-eye text-primary me-2"></i>View detail</a>
                                                            </small>
                                                            <small class="w-50 text-center py-2">
                                                                <a class="text-body" onclick="add(${product.id})"><i class="fa fa-shopping-bag text-primary me-2"></i>Add to cart</a>
                                                            </small>
                                                        </div>
                                                        </div>
                                                        </div>



                                            `;
                            $(targetContentId).append(productHtml);


                        };
                        $(targetContentId).append(`
                                        <div class="col-12 text-center" >
                                    <a class="btn btn-primary rounded-pill py-3 px-5" href="/Product/GetAll"> Browse More Products </a>
                                    </div>`
                        );
                    } else {
                        $('#' + targetContentId).html('<p>No products found.</p>');
                    }
                },
                error: function (xhr, status, error) {
                    // Handle errors
                    console.error('Error:', xhr.responseText);
                }
            });
        });
});



