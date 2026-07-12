import { useEffect, useState } from "react";
import api from "../api/axios";

export default function StudentDashboard() {

    const [books, setBooks] = useState([]);
    const [myBooks, setMyBooks] = useState([]);

    async function loadBooks() {
        const response = await api.get("/books");
        setBooks(response.data);
    }

    async function loadMyBooks() {
        const response = await api.get("/books/mybooks");
        setMyBooks(response.data);
    }

    async function borrowBook(bookId) {
        try {
            await api.post(`/books/${bookId}/borrow`);

            loadBooks();
            loadMyBooks();
        }
        catch {
            alert("Borrow failed");
        }
    }

    async function returnBook(bookId) {
        try {
            await api.post(`/books/${bookId}/return`);

            loadBooks();
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

            <button onClick={() => {
                localStorage.clear();
                window.location.href = "/";
            }}>
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
                        <th></th>
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

            <hr />

            <h2>My Borrowed Books</h2>

            <table border="1" cellPadding="8">

                <thead>

                    <tr>

                        <th>Book</th>

                        <th>Borrow Date</th>

                        <th></th>

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