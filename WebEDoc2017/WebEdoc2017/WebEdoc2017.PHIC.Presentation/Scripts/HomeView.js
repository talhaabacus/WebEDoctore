/* Tree View Script*/

$(document).ready(function () {

    $(".treeview li > ul").css('display', 'none');
    $(".collapsible").click(function (e) {

        e.preventDefault();
        $(this).toggleClass("collapse expand");
        $(this).closest("li").children("ul").slideToggle();
    });




});



$(function () {
    $('.selector').contextPopup({

        title: 'Document Category',
        items: [
          {
              label: 'Create Sub-Category', icon: '../WebEdoc2017.PHIC.Presentation/Images/Category.png', action: function ($trigger, e) {
                  var triggerElement = $trigger.currentTarget;
                  getSelectedNodeValue(triggerElement, "SubCategory");
              }
          },
          {
              label: 'Create Sibling', icon: '../WebEdoc2017.PHIC.Presentation/Images/Category.png', action: function ($trigger, e) {

                  var triggerElement = $trigger.currentTarget;
                  getSelectedNodeValue(triggerElement, "Sibling");
              }
          },
          {
              label: 'Edit', icon: '../WebEdoc2017.PHIC.Presentation/Images/edit.png', action: function ($trigger, e) {
                  var triggerElement = $trigger.currentTarget;
                  getSelectedNodeValue(triggerElement, "Edit");
                  //var Self = $trigger.currentTarget;
                  //var Parent = $trigger.currentTarget;

                  //var MenuID = $("#" + Self.id).find("input:hidden[name=MenuID]").val();
                  //var ParentID = $("#" + Parent.id).find("input:hidden[name=ParentID]").val();

                  //$("#txtMenuID").val(MenuID);
                  //  $("#txtParentID").val(ParentID);
              }
          },

          {
              label: 'Delete', icon: '../WebEdoc2017.PHIC.Presentation/Images/delete.png', action: function ($trigger, e) {

                  var triggerElement = $trigger.currentTarget;
                  getSelectedNodeValue(triggerElement, "Delete");
              }
          },
          //{ label: 'Cheese', icon: '../../contextmenumaster/icons/bin-metal.png', action: function () { alert('clicked 5') } },
          //{ label: 'Bacon', icon: '../../Images/bullet.png', action: function () { alert('clicked 6') } },
        //  null, // divider
          //{ label: 'Onwards', icon: '../../Images/bullet.png', action: function () { alert('clicked 7') } },
          //{ label: 'Flutters', icon: '../../contextmenumaster/icons/cassette.png', action: function () { alert('clicked 8') } }
        ]
    });


});



function getSelectedNodeValue(CurrentID, type) {
    $("#txtCategoryName").val("");
    $("#txtCategoryDesc").val("");

    if (type == "Delete") {
        $('#DeleteCategoryConfirmation').dialog('open');
    }
    else {
        if (type == "Edit") {
            $('#AddEditCategory').dialog('option', 'title', 'Edit Record');
            var Name = $("#" + CurrentID.id).find("input:hidden[name=Name]").val();
            var Description = $("#" + CurrentID.id).find("input:hidden[name=Description]").val();
            $("#txtCategoryName").val(Name);
            $("#txtCategoryDesc").val(Description);
        }

        else
            $('#AddEditCategory').dialog('option', 'title', 'Add Sibling/Sub-Category');




        $('#AddEditCategory').dialog('open');

    }


    var MenuID = $("#" + CurrentID.id).find("input:hidden[name=MenuID]").val();
    var ParentID = $("#" + CurrentID.id).find("input:hidden[name=ParentID]").val();
   

    $("#_MenuID").val(MenuID);
    $("#_ParentID").val(ParentID);
   
    $("#actionType").val(type);

}

function GetPatientDocument(MenuID, ParentID) {


    var par = {
        'MenuID': MenuID,
        'ParentID': ParentID,
    };

    $.ajax({
        url: "Home/getPatientDocumentByCategoryID",
        type: "POST",
        data: par,
        success: function (response, textStatus, jqXHR) {

            $("#PatientDocumentContent").html("");
            $("#PatientDocumentContent").html(response);

        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("error");
        },
        // callback handler that will be called on completion
        // which means, either on success or error
        complete: function () {
        }
    });
}



$(function () {
    $('#DeleteCategoryConfirmation').dialog({
        title: "Confirmation",
        autoOpen: false,
        width: 400,
        modal: true,
        height: 150,
        buttons: {
            "Delete": function () {
                DeleteCategory();
            },
            "Close": function () { $(this).dialog("close"); }
        }

    });
});

$(function () {
    $('#AddEditCategory').dialog({
        title: "Add/Edit",
        autoOpen: false,
        width: 400,
        modal: true,
        height: 300,
        buttons: {
            "SAVE": function () {
                SaveCategory();
            },
            "Close": function () { $(this).dialog("close"); }
        }

    });
});

function SaveCategory() {

    var _MenuID = $("#_MenuID").val();
    var _ParentID = $("#_ParentID").val();
    var actionType = $("#actionType").val();
    var CategoryName = $("#txtCategoryName").val();
    var CategoryDesc = $("#txtCategoryDesc").val();

    var URL = 'Home/SaveCategory';

    var parameter = {
        '_MenuID': _MenuID,
        '_ParentID': _ParentID,
        'actionType': actionType,
        'CategoryName': CategoryName,
        'CategoryDesc': CategoryDesc

    };
    $.post(URL, parameter, function (data, status, xhr) {
        if (data != null) {
            $("#divTreeView").html("");
            $("#divTreeView").html(data);

            $("#txtCategoryName").val("");
            $("#txtCategoryDesc").val("");
            $("#AddEditCategory").dialog("close")
        }



    })

}


function DeleteCategory() {
    var CategoryID = $("#_MenuID").val();

    var URL = 'Home/DeleteDocumentCategoryByCategoryID';

    var parameter = {
        'CategroyID': CategoryID,


    };
    $.post(URL, parameter, function (data, status, xhr) {
        if (data != null) {
            $("#divTreeView").html("");
            $("#divTreeView").html(data);
            $("#DeleteCategoryConfirmation").dialog("close")
        }



    })
}
jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();