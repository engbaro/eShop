var baseURL='http://localhost:52134/';
$(document).ready(function () {




  //ajax view category
    $(".list-group-item").click(function () {

        var id = $(this).attr('id');
    $.ajax({
        url: '/CustProducts/ViewCategory/',
      method: 'get',
      data:{ "id": id },
      success: function (data) {
          $('#page-content').html(data);
          window.history.pushState('1', "page 2", baseURL + 'CustProducts/ViewCategory/1');
      }

    });//end of ajax
   
  });//end of click




    //ajax add product
    

    $(".view-product").click(function () {
        var id = $(this).attr('id');
        $.ajax({
        url: '/CustProducts/ViewProduct',
        method: 'get',
        data:{'id':id},
        success: function (data) {
          $('#page-content').html(data);
        }
    });//end of ajax

  });//end of click

  //ajax edit product
  $("#edit-product").click(function () {
    $.ajax({
      url: '/AdminProducts/Edit',
      method: 'get',
      success: function (data) {
          $('#page-content').html(data);
      }
    });//end of ajax call

  });//end of click


//orders ajax

  $("#view-orders").click(function () {
      $.ajax({
          url: '/AdminOrders/ViewAll',
          method: 'get',
          success: function (data) {
              $('#page-container').html(data);
          }
      });//end of ajax call

  });//end of click
  $("#add-orders").click(function () {
      $.ajax({
          url: '/AdminOrders/Create',
          method: 'get',
          success: function (data) {
              $('#page-container').html(data);
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
          $('#page-container').html(data);
      }
    });//end of ajax

  });//click

 
});//ready
