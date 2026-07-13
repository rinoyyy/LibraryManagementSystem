export default function BookForm({
    title,
    setTitle,
    author,
    setAuthor,
    publishedYear,
    setPublishedYear,
    totalCopies,
    setTotalCopies,
    availableCopies,
    setAvailableCopies,
    editingBookId,
    saveBook
}) {
    return (

        <div className="form-container">

            <h3>

                {editingBookId === null
                    ? "Add Book"
                    : "Edit Book"}

            </h3>

            <form onSubmit={saveBook}>

                <div className="mb-3">

                    <label className="form-label">

                        Title

                    </label>

                    <input
                        className="form-control"
                        value={title}
                        onChange={(e)=>setTitle(e.target.value)}
                    />

                </div>

                <div className="mb-3">

                    <label className="form-label">

                        Author

                    </label>

                    <input
                        className="form-control"
                        value={author}
                        onChange={(e)=>setAuthor(e.target.value)}
                    />

                </div>

                <div className="mb-3">

                    <label className="form-label">

                        Published Year

                    </label>

                    <input
                        className="form-control"
                        type="number"
                        value={publishedYear}
                        onChange={(e)=>setPublishedYear(e.target.value)}
                    />

                </div>

                <div className="mb-3">

                    <label className="form-label">

                        Total Copies

                    </label>

                    <input
                        className="form-control"
                        type="number"
                        value={totalCopies}
                        onChange={(e)=>setTotalCopies(e.target.value)}
                    />

                </div>

                {editingBookId !== null && (

                    <div className="mb-3">

                        <label className="form-label">

                            Available Copies

                        </label>

                        <input
                            className="form-control"
                            type="number"
                            value={availableCopies}
                            onChange={(e)=>setAvailableCopies(e.target.value)}
                        />

                    </div>

                )}

                <button
                    className="btn btn-success"
                    type="submit"
                >
                    {editingBookId === null
                        ? "Add Book"
                        : "Update Book"}
                </button>

            </form>

        </div>

    );
}