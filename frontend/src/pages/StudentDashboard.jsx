import { useEffect, useState } from "react";
import api from "../api/axios";

export default function StudentDashboard() {

    const [books, setBooks] = useState([]);
    const [myBooks, setMyBooks] = useState([]);

    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    async function loadBooks(page = pageNumber) {

        try {

            const response = await api.get(
                `/books?pageNumber=${page}&pageSize=5`
            );

            setBooks(response.data.items);
            setPageNumber(response.data.currentPage);
            setTotalPages(response.data.totalPages);

        }
        catch {

            alert("Failed to load books");

        }

    }

    async function loadMyBooks() {

        try {

            const response = await api.get("/books/mybooks");

            setMyBooks(response.data);

        }
        catch {

            alert("Failed to load borrowed books");

        }

    }

    async function borrowBook(bookId) {

        try {

            await api.post(`/books/${bookId}/borrow`);

            loadBooks(pageNumber);
            loadMyBooks();

        }
        catch {

            alert("Borrow failed");

        }

    }

    async function returnBook(bookId) {

        try {

            await api.post(`/books/${bookId}/return`);

            loadBooks(pageNumber);
            loadMyBooks();

        }
        catch {

            alert("Return failed");

        }

    }

    useEffect(() => {

        loadBooks();
        loadMyBooks();

    }, []);

    return (

        <div style={{ padding: 20 }}>

            <h1>Student Dashboard</h1>

            <button
                onClick={() => {
                    localStorage.clear();
                    window.location.href = "/";
                }}
            >
                Logout
            </button>

            <hr />

            <h2>Available Books</h2>

            <table border="1" cellPadding="8">

                <thead>

                    <tr>

                        <th>Title</th>
                        <th>Author</th>
                        <th>Available</th>
                        <th>Action</th>

                    </tr>

                </thead>

                <tbody>

                    {books.map(book => (

                        <tr key={book.id}>

                            <td>{book.title}</td>
                            <td>{book.author}</td>
                            <td>{book.availableCopies}</td>

                            <td>

                                <button
                                    disabled={book.availableCopies === 0}
                                    onClick={() => borrowBook(book.id)}
                                >
                                    Borrow
                                </button>

                            </td>

                        </tr>

                    ))}

                </tbody>

            </table>

            <br />

            <button
                disabled={pageNumber === 1}
                onClick={() => loadBooks(pageNumber - 1)}
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
                onClick={() => loadBooks(pageNumber + 1)}
            >
                Next
            </button>

            <hr />

            <h2>My Borrowed Books</h2>

            <table border="1" cellPadding="8">

                <thead>

                    <tr>

                        <th>Book</th>
                        <th>Borrow Date</th>
                        <th>Action</th>

                    </tr>

                </thead>

                <tbody>

                    {myBooks.map(book => (

                        <tr key={book.borrowRecordId}>

                            <td>{book.bookTitle}</td>

                            <td>{book.borrowDate}</td>

                            <td>

                                <button
                                    onClick={() => returnBook(book.bookId)}
                                >
                                    Return
                                </button>

                            </td>

                        </tr>

                    ))}

                </tbody>

            </table>

        </div>

    );

}