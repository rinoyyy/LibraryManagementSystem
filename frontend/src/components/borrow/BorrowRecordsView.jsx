import Pagination from "../common/Pagination";

export default function BorrowRecordsView({
    borrowRecords,
    pageNumber,
    totalPages,
    loadBorrowRecords
}) {

    return (
        <>

            <div className="table-container">

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