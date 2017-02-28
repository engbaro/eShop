var hash = {
    '.png': 1,
    '.jpg': 1,
    '.PNG': 1,
    '.JPG': 1,
};


$(document).ready(function () {

    $("#add-more-photos").click(function () {
        var fileCount = $('.files').length;
        if (fileCount< 6) {
            var id = fileCount + 1;
     
            $(".add-more-photos-div").append('<div><input type="radio" name="Main" class="main-image" id="main-' + id + '" disabled/><label for="main-' + id + '"></label><input type="file" name="productImage" id="' + id + '" class="files"/> <br/></div>');
        }
    });
    $('.form-horizontal').on("change", 'input:file', function (e) {
        debugger;
        var id = e.target.id;
        
        var re = /\..+$/;
        var ext = e.target.files[0].name.match(re);
        if (hash[ext]) {
            $("#submit-product-photo").attr("disabled",false);
            $("#main-"+id).attr("disabled", false);
            return true;
        } else {
            alert("Please uploas an image of type png, jpg.");
            $("#submit-product-photo").attr("disabled", true);
            $("#main-" + id).attr("disabled", true);
            return false;
        }
    });
    $('#upload-photos-form').submit(function () {
        if ($('.files').length==0) {
            alert('you should upload at least one photo for your product');
            return false;
        }
        var id = $('input[name=Main]:checked', '#upload-photos-form').attr('id').split('-');
        $('input[name=Main]:checked', '#upload-photos-form').val($('#' + id[1]).val().split('\\').pop());
        return true;
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
