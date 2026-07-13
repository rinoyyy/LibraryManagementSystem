import Pagination from "../common/Pagination";

export default function BorrowRecordsView({
    borrowRecords,
    pageNumber,
    totalPages,
    loadBorrowRecords,
    search,
    setSearch,
    borrowDate,
    setBorrowDate
}) {

    return (
        <>

            <div className="table-container">

            <div className="row mb-3">

    <div className="col-md-6">

        <input
            type="text"
            className="form-control"
            placeholder="Search Student, Book or Author..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
        />

    </div>

    <div className="col-md-3">

        <input
            type="date"
            className="form-control"
            value={borrowDate}
            onChange={(e) => setBorrowDate(e.target.value)}
        />

    </div>

    <div className="col-md-3 d-flex gap-2">

        <button
            className="btn btn-primary"
            onClick={() => loadBorrowRecords(1)}
        >
            Search
        </button>

        <button
            className="btn btn-secondary"
            onClick={() => {

                setSearch("");

                setBorrowDate("");

                loadBorrowRecords(1);

            }}
        >
            Clear
        </button>

    </div>

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