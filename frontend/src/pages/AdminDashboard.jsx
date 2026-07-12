import { useEffect, useState } from "react";
import api from "../api/axios";

export default function AdminDashboard() {

    const [books, setBooks] = useState([]);
    const [borrowRecords, setBorrowRecords] = useState([]);

    const [pageNumber, setPageNumber] = useState(1);

    const [totalPages, setTotalPages] = useState(1);

    const [title, setTitle] = useState("");
    const [author, setAuthor] = useState("");
    const [publishedYear, setPublishedYear] = useState("");
    const [totalCopies, setTotalCopies] = useState("");
    const [editingBookId, setEditingBookId] = useState(null);
    const [availableCopies, setAvailableCopies] = useState("");

    async function loadBooks() {
        const response = await api.get("/books");
        setBooks(response.data);
    }

    async function loadBorrowRecords(page = pageNumber) {

    try {

        const response = await api.get(
            `/borrowrecords?pageNumber=${page}&pageSize=5`
        );

        setBorrowRecords(response.data.items);

        setPageNumber(response.data.currentPage);

        setTotalPages(response.data.totalPages);

    }
    catch {

        alert("Failed to load borrow records");

    }

}

    async function saveBook(e) {

    e.preventDefault();

    try {

        if (editingBookId === null) {

            await api.post("/books", {
                title,
                author,
                publishedYear: Number(publishedYear),
                totalCopies: Number(totalCopies)
            });

        }
        else {

            await api.put(`/books/${editingBookId}`, {

                title,
                author,
                publishedYear: Number(publishedYear),
                totalCopies: Number(totalCopies),
                availableCopies: Number(availableCopies)

            });

        }

        setEditingBookId(null);

        setTitle("");
        setAuthor("");
        setPublishedYear("");
        setTotalCopies("");
        setAvailableCopies("");

        loadBooks();

    }
    catch {

        alert("Operation failed");

    }

}

    function editBook(book) {

        setEditingBookId(book.id);

        setTitle(book.title);
        setAuthor(book.author);
        setPublishedYear(book.publishedYear);
        setTotalCopies(book.totalCopies);
        setAvailableCopies(book.availableCopies);
    }

    async function deleteBook(id) {

        if (!window.confirm("Delete this book?"))
            return;

        try {

            await api.delete(`/books/${id}`);

            loadBooks();

        }
        catch {

            alert("Delete failed");

        }

    }

    useEffect(() => {

    loadBooks();

    loadBorrowRecords();

}, []);

    return (

        <div style={{ padding: 20 }}>

            <h1>Admin Dashboard</h1>

            <button
                onClick={() => {

                    localStorage.clear();

                    window.location.href = "/";

                }}
            >
                Logout
            </button>

            <hr />

            <h2>Add Book</h2>

            <form onSubmit={saveBook}>

                <input
                    placeholder="Title"
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                />

                <br /><br />

                <input
                    placeholder="Author"
                    value={author}
                    onChange={(e) => setAuthor(e.target.value)}
                />

                <br /><br />

                <input
                    placeholder="Published Year"
                    value={publishedYear}
                    onChange={(e) => setPublishedYear(e.target.value)}
                />

                <br /><br />

                <input
                    placeholder="Total Copies"
                    value={totalCopies}
                    onChange={(e) => setTotalCopies(e.target.value)}
                />

                <br /><br />

                {editingBookId !== null && (

    <>
        <br /><br />

        <input
            placeholder="Available Copies"
            value={availableCopies}
            onChange={(e) => setAvailableCopies(e.target.value)}
        />
    </>

)}<br/><br/>

                <button type="submit">

    {editingBookId === null ? "Add Book" : "Update Book"}

</button>

            </form>

            <hr />

            <h2>Books</h2>

            <table border="1" cellPadding="8">

                <thead>

                    <tr>

                        <th>Title</th>

                        <th>Author</th>

                        <th>Year</th>

                        <th>Available</th>

                        <th>Total</th>

                        <th></th>

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
        onClick={() => editBook(book)}
    >
        Edit
    </button>

    {" "}

    <button
        onClick={() => deleteBook(book.id)}
    >
        Delete
    </button>

</td>

                        </tr>

                    ))}

                </tbody>

            </table>

            <hr />

<h2>Borrow Records</h2>

<table border="1" cellPadding="8">

    <thead>

        <tr>

            <th>Student</th>

            <th>Book</th>

            <th>Borrow Date</th>

            <th>Return Date</th>

            <th>Status</th>

        </tr>

    </thead>

    <tbody>

        {borrowRecords.map(record => (

            <tr key={record.borrowRecordId}>

                <td>{record.studentName}</td>

                <td>{record.bookTitle}</td>

                <td>{record.borrowDate}</td>

                <td>{record.returnDate ?? "-"}</td>

                <td>{record.status}</td>

            </tr>

        ))}

    </tbody>

</table>

<br />

<button
    disabled={pageNumber === 1}
    onClick={() => loadBorrowRecords(pageNumber - 1)}
>
    Previous
</button>

{" "}

<span>

    Page {pageNumber} of {totalPages}

</span>

{" "}

<button
    disabled={pageNumber === totalPages}
    onClick={() => loadBorrowRecords(pageNumber + 1)}
>
    Next
</button>

        </div>

    );

}