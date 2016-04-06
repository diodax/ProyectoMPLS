
$(document).ready(function () {
    var $$ = go.GraphObject.make;
    //var myDiagram = $(go.Diagram, "myDiagramDiv");

    var myDiagram = $$(go.Diagram, "myDiagramDiv", {
        initialContentAlignment: go.Spot.Center,        // center Diagram contents
        "undoManager.isEnabled": true,                   // enable Ctrl-Z to undo and Ctrl-Y to redo
        //layout: $(go.TreeLayout,                        // specify a Diagram.layout that arranges trees
        //    { angle: 90, layerSpacing: 35 })
    });

    myDiagram.allowDrop = true;  // permit accepting drag-and-drops

    // when the document is modified, add a "*" to the title and enable the "Save" button
    myDiagram.addDiagramListener("Modified", function (e) {
        var button = document.getElementById("save");
        if (button) button.disabled = !myDiagram.isModified;
        var idx = document.title.indexOf("*");
        if (myDiagram.isModified) {
            if (idx < 0) document.title += "*";
        } else {
            if (idx >= 0) document.title = document.title.substr(0, idx);
        }
    });

    // define a simple Node template
    myDiagram.nodeTemplate =
        $$(go.Node, "Auto",
        // the entire node will have a light-blue background
        //{ background: "#44CCFF" },
        $$(go.Picture,
            // Pictures should normally have an explicit width and height.
            // This picture has a red background, only visible when there is no source set
            // or when the image is partially transparent.
            { margin: 10, width: 80, height: 80 },
            // Picture.source is data bound to the "source" attribute of the model data
            new go.Binding("source")),
        $$(go.TextBlock,
            "Router1",  // the initial value for TextBlock.text
            // some room around the text, a larger font, and a white stroke:
            { margin: 12, stroke: "black", font: "bold 16px sans-serif", editable: true },
            // TextBlock.text is data bound to the "name" attribute of the model data
            new go.Binding("text", "name"))
        );
    
    // funcion que abre modal de _ConfigEnlace
    function abrirConfigEnlace(e, obj) {
        //window.open('/Topologia/_ConfigEnlace?idEnlace=1&idProyecto=79', '_self', 'datamodal=""');

        //var idEnlace = $('#tbxIdEnlace').val();
        //console.log(idEnlace);
        var idProyecto = $('#tbxIdProyecto').val();
        
        //console.log(obj);

        // console.log(idProyecto);
        //console.log(obj.data.from);
        //console.log(obj.data.to);
        idEnlace = obj.data.idEnlace;
        //console.log(idEnlace);
        var href = '/Topologia/_ConfigEnlace?idEnlace=' + idEnlace + '&idProyecto=' + idProyecto;

        // hide dropdown if any
        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');

        $('#myModalContent').load(href, function () {
            $('#myModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');

            bindForm(this);
        });

        return false;
    }

    // define a Link template that routes orthogonally, with no arrowhead
    myDiagram.linkTemplate =
      $$(go.Link,
        { routing: go.Link.Normal, corner: 5 },
        $$(go.Shape, { strokeWidth: 3, stroke: "#555" }),
        { contextClick: abrirConfigEnlace }
        //{ idEnlace: 0 }
        ); // the link shape

    var image = '/Content/Images/' + 'router.png';
    var model = $$(go.GraphLinksModel);

    $.ajax({
        url: '/Topologia/GetJsonTopologia',
        //type: 'POST',
        contentType: "application/json",
        //datatype: "json",
        data: { idProyecto: $("#tbxIdProyecto").val() },
        success: function (result) {
            //Inicio AJAX --
            var image2 = '/Content/Images/' + 'router.png';
            //var model = $$(go.GraphLinksModel);

            //alert("working");
            //console.log(result.routers);
            //console.log(result.enlaces);

                
            var arrayRouters = [];
            $.each(result.routers, function (i, item) {
                arrayRouters.push({ "key": result.routers[i].idRouter, "name": result.routers[i].cHostname, "source": image2 });
            });

            //$(result.routers).each(function () {
            //    arrayRouters.push({ "key": $(this).idRouter, "name": $(this).cHostname, source: image2 });
            //});
            model.nodeDataArray = arrayRouters;

            var arrayLinks = [];
            //console.log(result.enlaces);
            $.each(result.enlaces, function (i, item) {
                // console.log(result.enlaces);
                //console.log("from: " + result.enlaces[i].idRouterA);
                //console.log("to: " + result.enlaces[i].idRouterB);
                //console.log("idEnlace: " + result.enlaces[i].idEnlace);
                arrayLinks.push({ "from": result.enlaces[i].idRouterA, "to": result.enlaces[i].idRouterB, toArrow: "", "idEnlace": result.enlaces[i].idEnlace});
            });

            //$(result.enlaces).each(function () {
            //    arrayLinks.push({ "from": $(this).idRouterA, "to": $(this).idRouterB });
            //});
            //console.log(arrayRouters);
            //console.log(arrayLinks);
            //

            model.linkDataArray = arrayLinks;
            myDiagram.model = model;
               
            //Fin AJAX --
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus);
            console.log(errorThrown);
            $("#error").html(jqXHR.responseText);
        }
    });

    var myPalette = $$(go.Palette, "myPaletteDiv");

    // the Palette's node template is different from the main Diagram's
    myPalette.nodeTemplate =
        $$(go.Node, "Horizontal",
        $$(go.Picture,
            { width: 40, height: 40 },
            new go.Binding("source")),
        $$(go.TextBlock,
            new go.Binding("text", "color"))
        );

    // the list of data to show in the Palette
    myPalette.model.nodeDataArray = [
        { source: image, color: "LER" },
        { source: image, color: "LSR" },
        //{ source: image, color: "aquamarine" },
        //{ source: image, color: "turquoise" },
        //{ source: image, color: "powderblue" },
        //{ source: image, color: "lightblue" },
        //{ source: image, color: "lightskyblue" },
        //{ source: image, color: "deepskyblue" }
    ];

    $('#file').click(function () {
        //myDiagram.commandHandler.
    })

    $('#save').click(function () {
        //document.getElementById("mySavedModel").value = myDiagram.model.toJson();
        var result = [];

        $.ajax({
            url: '/Topologia/SetJsonTopologia',
            type: 'POST',
            contentType: "application/json",
            //datatype: "json",
            beforeSend: function () {
                var arrayRouters = model.nodeDataArray;
                //result = [];

                $.each(arrayRouters, function (i, item) {
                    result.push({ "idRouter": arrayRouters[i].key, "cHostname": arrayRouters[i].name });
                });

                //console.log(result);
                //return result;
                //idProyecto: $("#tbxIdProyecto").val()
            },
            data: {
                result
            },
            success: function () {
                //Inicio AJAX --
                /*var arrayRouters = model.nodeDataArray;
                result = [];

                $.each(arrayRouters, function (i, item) {
                    result.push({ "idRouter": arrayRouters[i].key, "cHostname": arrayRouters[i].name });
                });

                console.log(result);*/
                //Fin AJAX --
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus);
                console.log(errorThrown);
                $("#error").html(jqXHR.responseText);
            }
        });

        // TODO cambiar linea anterior por metodo que vayamos a usar para grabar
        myDiagram.isModified = false;
    })

    $('#cut').click(function () {
        myDiagram.commandHandler.cutSelection();
    })

    $('#copy').click(function () {
        myDiagram.commandHandler.copySelection();
    })

    $('#paste').click(function () {
        myDiagram.commandHandler.pasteFromClipboard();
    })    

    $('#undo').click(function () {
        myDiagram.commandHandler.undo();
    })
    
    $('#redo').click(function () {
        myDiagram.commandHandler.redo();
    })

    $('#delete').click(function () {
        myDiagram.commandHandler.deleteSelection();
    })
});