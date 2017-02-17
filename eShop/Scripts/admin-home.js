var hash = {
    '.png': 1,
    '.jpg': 1,
    '.PNG': 1,
    '.JPG': 1,
};


$(document).ready(function () {

    $('input[type="file"]').change(function (e) {
        debugger;
        var re = /\..+$/;
        var ext = e.target.files[0].name.match(re);
        if (hash[ext]) {
            $("#submit-product-photo").attr("disabled",false);
            return true;
        } else {
            alert("Please uploas an image of type png, jpg.");
            $("#submit-product-photo").attr("disabled", true);

            return false;
        }
    });

  //ajax view product
  $("#view-products").click(function () {
    $.ajax({
      url: '/AdminProducts/ViewAll',
      method: 'get',
      success: function (data) {
        $('#page-wrapper').html(data);
      }
    });//end of ajax

  });//end of click

  //ajax add product
  $("#add-product").click(function () {
    $.ajax({
      url: '/AdminProducts/Create',
      method: 'get',
      success: function (data) {
        $('#page-wrapper').html(data);
      }
    });//end of ajax

  });//end of click

  //ajax edit product
  $("#edit-product").click(function () {
    $.ajax({
      url: '/AdminProducts/Edit',
      method: 'get',
      success: function (data) {
        $('#page-wrapper').html(data);
      }
    });//end of ajax call

  });//end of click


//orders ajax

  $("#view-orders").click(function () {
      $.ajax({
          url: '/AdminOrders/ViewAll',
          method: 'get',
          success: function (data) {
              $('#page-wrapper').html(data);
          }
      });//end of ajax call

  });//end of click
  $("#add-orders").click(function () {
      $.ajax({
          url: '/AdminOrders/Create',
          method: 'get',
          success: function (data) {
              $('#page-wrapper').html(data);
          }
      });//end of ajax call

  });//end of click



//end of orders ajax
  //dashboard
  $("#dashboard").click(function () {
    $.ajax({
      url: '/AdminMain/DashBoard',
      method: 'get',
      success: function (data) {
        $('#page-wrapper').html(data);
      }
    });//end of ajax

  });//click

 
});//ready
