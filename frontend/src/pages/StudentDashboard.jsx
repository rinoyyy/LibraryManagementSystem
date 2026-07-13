import { useEffect, useState } from "react";
import api from "../api/axios";
import DashboardLayout from "../layouts/DashboardLayout";
import DashboardCard from "../components/dashboard/DashboardCard";

export default function StudentDashboard() {

    const [activeItem, setActiveItem] = useState("Dashboard");

    const [stats, setStats] = useState({
        availableBooks: 0,
        borrowedBooks: 0,
        returnedBooks: 0,
        totalBorrowed: 0
    });
    const [books, setBooks] = useState([]);

    const [myBooks, setMyBooks] = useState([]);

    const [history, setHistory] = useState([]);

const [historyPage, setHistoryPage] = useState(1);

const [historyTotalPages, setHistoryTotalPages] = useState(1);

const [myBooksPage, setMyBooksPage] = useState(1);

const [myBooksTotalPages, setMyBooksTotalPages] = useState(1);

const [pageNumber, setPageNumber] = useState(1);

const [totalPages, setTotalPages] = useState(1);

    async function loadDashboard() {

        try {

            const response = await api.get("/dashboard/student");

            setStats(response.data);

        }
        catch {

            console.log("Failed to load dashboard");

        }

    }

    async function loadBooks(page = 1) {

    try {

        const response = await api.get(
            `/books?pageNumber=${page}&pageSize=5`
        );

        setBooks(response.data.items);
        setPageNumber(response.data.currentPage);
        setTotalPages(response.data.totalPages);

    }
    catch {

        console.log("Failed to load books");

    }

}

async function loadMyBooks(page = 1) {

    try {

        const response = await api.get(
            `/books/mybooks?pageNumber=${page}&pageSize=5`
        );

        setMyBooks(response.data.items);

        setMyBooksPage(response.data.currentPage);

        setMyBooksTotalPages(response.data.totalPages);

    }
    catch {

        console.log("Failed to load my books");

    }

}

async function loadHistory(page = 1) {

    try {

        const response = await api.get(
            `/books/history?pageNumber=${page}&pageSize=5`
        );

        setHistory(response.data.items);

        setHistoryPage(response.data.currentPage);

        setHistoryTotalPages(response.data.totalPages);

    }
    catch {

        console.log("Failed to load history");

    }

}

async function borrowBook(bookId) {

    try {

        await api.post(`/books/${bookId}/borrow`);

        await loadDashboard();

        await loadBooks(pageNumber);

        await loadMyBooks(myBooksPage);

        await loadHistory(historyPage);

    }
    catch {

        alert("Borrow failed");

    }

}

async function returnBook(bookId) {

    try {

        await api.post(`/books/${bookId}/return`);

        await loadDashboard();

        await loadBooks(pageNumber);

        await loadMyBooks(myBooksPage);

        await loadHistory(historyPage);

    }
    catch {

        alert("Return failed");

    }

}

    useEffect(() => {

    loadDashboard();

    loadBooks();

    loadMyBooks();

    loadHistory();

}, []);

    function renderContent() {

        switch (activeItem) {

            case "Dashboard":

                return (

                    <div className="row g-4">

                        <DashboardCard
                            title="Available Books"
                            value={stats.availableBooks}
                            icon="bi-book"
                            color="primary"
                        />

                        <DashboardCard
                            title="Borrowed Books"
                            value={stats.borrowedBooks}
                            icon="bi-journal-bookmark"
                            color="success"
                        />

                        <DashboardCard
                            title="Returned Books"
                            value={stats.returnedBooks}
                            icon="bi-arrow-return-left"
                            color="warning"
                        />

                        <DashboardCard
                            title="Total Borrowed"
                            value={stats.totalBorrowed}
                            icon="bi-collection"
                            color="dark"
                        />

                    </div>

                );

            case "Available Books":

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

                                <td>

                                    <button
                                        className="btn btn-success btn-sm"
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

            </div>

            <div className="d-flex justify-content-center mt-3">

                <button
                    className="btn btn-secondary me-2"
                    disabled={pageNumber === 1}
                    onClick={() => loadBooks(pageNumber - 1)}
                >
                    Previous
                </button>

                <span className="align-self-center">

                    Page {pageNumber} of {totalPages}

                </span>

                <button
                    className="btn btn-secondary ms-2"
                    disabled={pageNumber === totalPages}
                    onClick={() => loadBooks(pageNumber + 1)}
                >
                    Next
                </button>

            </div>

        </>

    );

            case "My Books":

    return (

        <>

            <div className="table-container">

                <table className="table table-hover">

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

                                <td>
                                    {new Date(book.borrowDate).toLocaleDateString()}
                                </td>

                                <td>

                                    <button
                                        className="btn btn-warning btn-sm"
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

            <div className="d-flex justify-content-center mt-3">

                <button
                    className="btn btn-secondary me-2"
                    disabled={myBooksPage === 1}
                    onClick={() => loadMyBooks(myBooksPage - 1)}
                >
                    Previous
                </button>

                <span className="align-self-center">

                    Page {myBooksPage} of {myBooksTotalPages}

                </span>

                <button
                    className="btn btn-secondary ms-2"
                    disabled={myBooksPage === myBooksTotalPages}
                    onClick={() => loadMyBooks(myBooksPage + 1)}
                >
                    Next
                </button>

            </div>

        </>

    );

            case "Borrow History":

    return (

        <>

            <div className="table-container">

                <table className="table table-hover">

                    <thead>

                        <tr>

                            <th>Book</th>

                            <th>Borrow Date</th>

                            <th>Return Date</th>

                            <th>Status</th>

                        </tr>

                    </thead>

                    <tbody>

                        {history.map(record => (

                            <tr key={record.borrowRecordId}>

                                <td>{record.bookTitle}</td>

                                <td>
                                    {new Date(record.borrowDate).toLocaleDateString()}
                                </td>

                                <td>

                                    {record.returnDate
                                        ? new Date(record.returnDate).toLocaleDateString()
                                        : "-"}

                                </td>

                                <td>

                                    <span
                                        className={
                                            record.status === "Borrowed"
                                                ? "badge bg-success"
                                                : "badge bg-secondary"
                                        }
                                    >
                                        {record.status}
                                    </span>

                                </td>

                            </tr>

                        ))}

                    </tbody>

                </table>

            </div>

            <div className="d-flex justify-content-center mt-3">

                <button
                    className="btn btn-secondary me-2"
                    disabled={historyPage === 1}
                    onClick={() => loadHistory(historyPage - 1)}
                >
                    Previous
                </button>

                <span className="align-self-center">

                    Page {historyPage} of {historyTotalPages}

                </span>

                <button
                    className="btn btn-secondary ms-2"
                    disabled={historyPage === historyTotalPages}
                    onClick={() => loadHistory(historyPage + 1)}
                >
                    Next
                </button>

            </div>

        </>

    );

            default:

                return null;

        }

    }

    return (

        <DashboardLayout

            title="Student Dashboard"

            username={localStorage.getItem("username")}

            menuItems={[
                "Dashboard",
                "Available Books",
                "My Books",
                "Borrow History"
            ]}

            activeItem={activeItem}

            setActiveItem={setActiveItem}

        >

            {renderContent()}

        </DashboardLayout>

    );

}