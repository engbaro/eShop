var baseURL='http://localhost:52134/';
$(document).ready(function () {

    	


        $('#login').popover({ 
            html : true,
            content: function() {
                return $("#login-form").html();
            }
        });
        $('#register').popover({
            html: true,
            content: function () {
                return $("#register-form").html();
            }
        });

    //ajax remove item
    $(".remove-item").click(function () {
        var tr = $(this).closest("tr");
        var id = $(this).attr('id');
        $.ajax({
            url: '/CustProducts/RemoveFromBasket/',
            method: 'post',
            data: { "id": id },
            success: function (data) {
                    $('.error').append( '<div class="alert alert-danger" role="alert">'+data.message+'</div>');
                   
                    if (data.empty) {
                        tr.closest('table').remove();
                    }
                    else {
                        tr.remove();
                    }
                window.history.pushState('1', "page 3", baseURL + 'CustProducts/ViewBasket/');
            }
        });//end of ajax

    });//end of click

  //ajax view category
    $(".list-group-item").click(function () {

        var id = $(this).attr('id');
    $.ajax({
        url: '/CustProducts/ViewCategory/',
      method: 'get',
      data:{ "id": id },
      success: function (data) {
          $('#page-content').html(data);
          window.history.pushState('1', "page 2", baseURL + 'CustProducts/ViewCategory/'+id);
      }

    });//end of ajax
   
  });//end of click


 
});//ready
