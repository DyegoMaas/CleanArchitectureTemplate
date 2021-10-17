# Encyclopedia Galactica - Project Specifications

The library contains only hand-picked books in text form, chosen by a select group of Editors. The Editors are the only ones who can manage the library's content.

A fellow reader can lend as many books as she want.

## Features

### F01 - Collection management

[ ] BR01 - An Editor can add a new book to the library collection.

[ ] BR02 - An Editor can remove a book from the library collection at any time. If some readers are reading this book, the next time they try to access it, it will not be available.

[ ] BR03 - An Editor can update the books content without notifying the author.

### F02 - Book lending

[ ] BR01 - A Reader can search books in the library collection.

[ ] BR02 - A Reader can visualize the public details of a book.

[ ] BR03 - A Reader can lend a book, downloading its contents full content.

## Non Functional Requirements

NFR01 - Collection metadata will be stored in a MongoDB 5 database.

NFR02 - Books will be storage in the local filesystem as UTF8.
