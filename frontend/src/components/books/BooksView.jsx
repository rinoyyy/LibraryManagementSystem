import Pagination from "../common/Pagination";

export default function BooksView({
    books,
    pageNumber,
    totalPages,
    loadBooks,
    editBook,
    deleteBook,
    search,
    setSearch
}) {
    return (
        <>
            <div className="table-container">

            <div className="input-group mb-3">

    <input
        type="text"
        className="form-control"
        placeholder="Search Title or Author..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        onKeyDown={(e) => {

            if (e.key === "Enter")
                loadBooks(1);

        }}
    />

    <button
        className="btn btn-primary"
        onClick={() => loadBooks(1)}
    >
        Search
    </button>

    <button
        className="btn btn-outline-secondary"
        onClick={() => {

            setSearch("");

            loadBooks(1);

        }}
    >
        Clear
    </button>

</div>

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