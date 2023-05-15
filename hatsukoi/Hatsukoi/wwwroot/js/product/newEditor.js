
ClassicEditor
    .create(document.querySelector('#editor'), {
        toolbar: {
            items: [
                'heading', '|', 'bold', 'italic', 'link', 'bulletedList', 'numberedList',
                '|', 'alignment', 'outdent', 'indent', '|', 'fontSize', 'fontColor',
                '|', 'imageUpload', 'blockQuote', 'insertTable', 'mediaEmbed',
                'undo', 'redo'
            ]
        },
        image: {
            toolbar: [
                'imageTextAlternative', 'imageStyle:inline',
                'imageStyle:block', 'imageStyle:side'
            ]
        },
        table: {
            contentToolbar: [
                'tableColumn', 'tableRow', 'mergeTableCells'
            ]
        },
        licenseKey: '',
        placeholder: '提供充足商品介紹資訊，不只可以讓買家更認識你的商品，減少回覆問題的時間，也能提升商品頁的曝光量。',
        heading: {
            // 設定 Heading 內的樣式，可新增多個
            options: [
                {
                    model: 'paragraph',
                    title: 'Paragraph',
                    class: 'ck-heading_paragraph'
                },
                {
                    model: 'heading1',
                    view: 'h2',
                    title: 'Heading 1',
                    class: 'ck-heading_heading1'
                },
                {
                    model: 'heading2',
                    view: 'h3',
                    title: 'Heading 2',
                    class: 'ck-heading_heading2'
                }
            ]
        },
    })
    .then(editor => {
        window.editor = editor;
        window.editorContent = editor.getData(); // 將 editorContent 定義在全域範圍內
        console.log(window.editorContent);
    })
    .catch(error => {
        console.error(error);
    });







