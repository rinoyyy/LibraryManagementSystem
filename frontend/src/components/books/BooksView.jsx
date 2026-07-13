import Pagination from "../common/Pagination";

export default function BooksView({
    books,
    pageNumber,
    totalPages,
    loadBooks,
    editBook,
    deleteBook
}) {
    return (
        <>
            <div className="table-container">

                <table className="table table-hover">

                    <thead>

                        <tr>
                            <th>Title</th>
                            <th>Author</th>
                            <th>Year</th>
                            <th>Available</th>
                            <th>Total</th>
                            <th>Actions</th>
                        </tr>

                    </thead>

                    <tbody>

                        {books.map(book => (

                            <tr key={book.id}>

                                <td>{book.title}</td>

                                <td>{book.author}</td>

                                <td>{book.publishedYear}</td>

                                <td>{book.availableCopies}</td>

                                <td>{book.totalCopies}</td>

                                <td>

                                    <button
                                        className="btn btn-primary btn-sm me-2"
                                        onClick={() => editBook(book)}
                                    >
                                        Edit
                                    </button>

                                    <button
                                        className="btn btn-danger btn-sm"
                                        onClick={() => deleteBook(book.id)}
                                    >
                                        Delete
                                    </button>

                                </td>

                            </tr>

                        ))}

                    </tbody>

                </table>

            </div>

            <Pagination
                pageNumber={pageNumber}
                totalPages={totalPages}
                onPrevious={() => loadBooks(pageNumber - 1)}
                onNext={() => loadBooks(pageNumber + 1)}
            />
        </>
    );
}