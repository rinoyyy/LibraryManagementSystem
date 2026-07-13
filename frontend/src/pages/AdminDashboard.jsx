import { useEffect, useState } from "react";
import api from "../api/axios";

import DashboardLayout from "../layouts/DashboardLayout";
import DashboardCard from "../components/dashboard/DashboardCard";

import BooksView from "../components/books/BooksView";
import BookForm from "../components/books/BookForm";
import BorrowRecordsView from "../components/borrow/BorrowRecordsView";

export default function AdminDashboard() {

    const [activeItem, setActiveItem] = useState("Dashboard");

    const [stats, setStats] = useState({
        totalBooks: 0,
        availableBooks: 0,
        borrowedBooks: 0,
        students: 0
    });

    const [books, setBooks] = useState([]);

    const [borrowRecords, setBorrowRecords] = useState([]);

    const [pageNumber, setPageNumber] = useState(1);

    const [totalPages, setTotalPages] = useState(1);

    const [title, setTitle] = useState("");

    const [author, setAuthor] = useState("");

    const [publishedYear, setPublishedYear] = useState("");

    const [totalCopies, setTotalCopies] = useState("");

    const [availableCopies, setAvailableCopies] = useState("");

    const [editingBookId, setEditingBookId] = useState(null);

    const [bookSearch, setBookSearch] = useState("");

    const [recordSearch, setRecordSearch] = useState("");

    

    async function loadDashboard() {

        const response = await api.get("/dashboard/admin");

        setStats(response.data);

    }

    async function loadBooks(page = 1) {

        const response = await api.get(
    `/books?pageNumber=${page}&pageSize=5&search=${encodeURIComponent(bookSearch)}`
);

        setBooks(response.data.items);

        setPageNumber(response.data.currentPage);

        setTotalPages(response.data.totalPages);

    }

    async function loadBorrowRecords(page = 1) {

    const response = await api.get(
        `/borrowrecords?pageNumber=${page}&pageSize=5&search=${encodeURIComponent(recordSearch)}`
    );

    setBorrowRecords(response.data.items);

    setPageNumber(response.data.currentPage);

    setTotalPages(response.data.totalPages);

}

    async function saveBook(e) {

        e.preventDefault();

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

        loadDashboard();
        loadBooks();

    }

    function editBook(book) {

        setEditingBookId(book.id);

        setTitle(book.title);

        setAuthor(book.author);

        setPublishedYear(book.publishedYear);

        setTotalCopies(book.totalCopies);

        setAvailableCopies(book.availableCopies);

        setActiveItem("Add Book");

    }

    async function deleteBook(id) {

        if (!window.confirm("Delete this book?"))
            return;

        await api.delete(`/books/${id}`);

        loadDashboard();

        loadBooks();

    }

    useEffect(() => {

        loadDashboard();

        loadBooks();

        loadBorrowRecords();

    }, []);

    function renderContent() {

        switch (activeItem) {

            case "Dashboard":

                return (

                    <div className="row g-4">

                        <DashboardCard
                            title="Total Books"
                            value={stats.totalBooks}
                            icon="bi-book"
                            color="primary"
                        />

                        <DashboardCard
                            title="Available"
                            value={stats.availableBooks}
                            icon="bi-check-circle"
                            color="success"
                        />

                        <DashboardCard
                            title="Borrowed"
                            value={stats.borrowedBooks}
                            icon="bi-journal-bookmark"
                            color="warning"
                        />

                        <DashboardCard
                            title="Students"
                            value={stats.students}
                            icon="bi-people"
                            color="dark"
                        />

                    </div>

                );
                        case "Books":

                return (

                    <BooksView
    books={books}
    pageNumber={pageNumber}
    totalPages={totalPages}
    loadBooks={loadBooks}
    editBook={editBook}
    deleteBook={deleteBook}
    search={bookSearch}
    setSearch={setBookSearch}
/>

                );

            case "Add Book":

                return (

                    <BookForm
                        title={title}
                        setTitle={setTitle}
                        author={author}
                        setAuthor={setAuthor}
                        publishedYear={publishedYear}
                        setPublishedYear={setPublishedYear}
                        totalCopies={totalCopies}
                        setTotalCopies={setTotalCopies}
                        availableCopies={availableCopies}
                        setAvailableCopies={setAvailableCopies}
                        editingBookId={editingBookId}
                        saveBook={saveBook}
                    />

                );

            case "Borrow Records":

                return (

                    <BorrowRecordsView
    borrowRecords={borrowRecords}
    pageNumber={pageNumber}
    totalPages={totalPages}
    loadBorrowRecords={loadBorrowRecords}
    search={recordSearch}
    setSearch={setRecordSearch}
/>

                );

            default:

                return null;

        }

    }

    return (

        <DashboardLayout

            title="Admin Dashboard"

            username={localStorage.getItem("username")}

            menuItems={[
                "Dashboard",
                "Books",
                "Add Book",
                "Borrow Records"
            ]}

            activeItem={activeItem}

            setActiveItem={setActiveItem}

        >

            {renderContent()}

        </DashboardLayout>

    );

}