import Pagination from "../common/Pagination";

export default function BorrowRecordsView({
    borrowRecords,
    pageNumber,
    totalPages,
    loadBorrowRecords,
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
        placeholder="Search Student, Book, Author or Date..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        onKeyDown={(e) => {

            if (e.key === "Enter")
                loadBorrowRecords(1);

        }}
    />

    <button
        className="btn btn-primary"
        onClick={() => loadBorrowRecords(1)}
    >
        Search
    </button>

    <button
        className="btn btn-outline-secondary"
        onClick={() => {

            setSearch("");

            loadBorrowRecords(1);

        }}
    >
        Clear
    </button>

</div>

                <table className="table table-hover">

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

            <Pagination
                pageNumber={pageNumber}
                totalPages={totalPages}
                onPrevious={() => loadBorrowRecords(pageNumber - 1)}
                onNext={() => loadBorrowRecords(pageNumber + 1)}
            />

        </>
    );

}