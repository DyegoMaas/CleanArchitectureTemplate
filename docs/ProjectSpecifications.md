# Encyclopedia Galactica - Project Specifications

The library contains only hand-picked books in text form, chosen by a select group of Editors. The Editors are the only ones who can manage the library's content as they see fit.

A fellow reader can lend as many books as she want, and the library tracks reading behavior.

## Features

### F01 - Collection management

[ ] BR01 - An Editor can add a new book to the library collection.

[ ] BR02 - An Editor can remove a book from the library collection at any time. If some readers are reading this book, the next time they try to access it, it will not be available.

[ ] BR03 - An Editor can change the books content without notifying the author.

[ ] BR04 - An Editor can censor certain sensible bits of the texts. 

### F02 - Book lending

[ ] BR01 - A Reader can search books in the library collection.

[ ] BR02 - A Reader can visualize the public details of a book.

[ ] BR03 - A Reader can lend a book, downloading its contents in the `.sdb` (self-destroyable book) format, bound by the lending period limit.

## Non Functional Requirements

NFR01 - Collection metadata will be stored in a MongoDB 5 database.

NFR02 - Books will be storage in the local filesystem under the SDB format (self-destroyable book) with expiration disabled.

NFR03 - The Readers actually download a copy of the original SDB file with self-destruction enabled.

NFR04 - The SDB format self-destroys the next time a Reader tries to open it after expiration.

NFR05 - A SDB file can only be edited with the master cryptographic key.  