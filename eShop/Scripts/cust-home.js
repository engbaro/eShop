var baseURL='http://localhost:52134/';
$(document).ready(function () {

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

function registerSuccess(response) {
    debugger;
    /*$('#register-form').addClass('show');
    $('#register-form').removeClass('fade');*/
    
    $('.error').removeClass('hide').html(response.errorMessage);
}

function registerFailure() {
    /*$('#register-form').addClass('show');
    $('#register-form').removeClass('fade');*/
    $('#phoneError').text(response);
}

function onComplete() {
    debugger;
}
function loginSuccess(response) {
    debugger;
    /*$('#register-form').addClass('show');
    $('#register-form').removeClass('fade');*/

    $('.error').removeClass('hide').html(response.errorMessage);
}

function loginFailure() {
    /*$('#register-form').addClass('show');
    $('#register-form').removeClass('fade');*/
    $('#phoneError').text(response);
}

