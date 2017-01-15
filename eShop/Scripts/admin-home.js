$(document).ready(function () {


 

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
