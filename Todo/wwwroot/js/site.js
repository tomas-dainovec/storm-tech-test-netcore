// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    var $newItemModal = $('#newItemModal')
        .on('shown.bs.modal', function (e) {
            var todoListId = $(e.relatedTarget).data('todo-list-id');
            $('.modal-body form', this).load('/TodoItem/RenderCreateFieldsPartial?todoListId=' + todoListId);
        })
        .on('hide.bs.modal', function () {
            $('.modal-body form', this).empty();
        });

    var $todoList = $('#todoList');

    $('#newItemForm').submit(function () {
        var $this = $(this);
        var formData = $this.serialize();

        $.post('/TodoItem/HandleCreateForm', formData, function (data, _, jqXHR) {
            if (jqXHR.status == 201) {
                $newItemModal.modal('hide');

                $.get('/TodoList/RenderListItems' + window.location.search, function (listData) {
                    $('li:first', $todoList)
                        .siblings()
                        .remove();

                    $todoList.append(listData);
                });
            } else {
                $this.html(data);
            }
        });
        
        return false;
    });
});
