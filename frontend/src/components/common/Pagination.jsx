export default function Pagination({
    pageNumber,
    totalPages,
    onPrevious,
    onNext
}) {

    return (

        <div className="d-flex justify-content-center mt-3">

            <button
                className="btn btn-secondary me-2"
                disabled={pageNumber === 1}
                onClick={onPrevious}
            >
                Previous
            </button>

            <span className="align-self-center">

                Page {pageNumber} of {totalPages}

            </span>

            <button
                className="btn btn-secondary ms-2"
                disabled={pageNumber === totalPages}
                onClick={onNext}
            >
                Next
            </button>

        </div>

    );

}