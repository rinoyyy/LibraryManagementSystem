

export default function Sidebar({
    items,
    active,
    setActive,
    logout
}) {

    const icons = {
        Dashboard: "bi-speedometer2",
        "Available Books": "bi-book",
        "My Books": "bi-journal-bookmark",
        "Borrow History": "bi-clock-history",
        "View Books": "bi-book-half",
        "Add Book": "bi-plus-circle",
        "Borrow Records": "bi-card-list"
    };

    return (
        <div
            className="sidebar bg-dark text-white d-flex flex-column p-3"
            style={{
                width: "260px",
                minHeight: "100vh"
            }}
        >
            <h3 className="text-center mb-4">
                📚 Library
            </h3>

            {items.map(item => (

                <button
                    key={item}
                    className={
                        `btn text-start mb-2 ${
                            active === item
                                ? "btn-primary"
                                : "btn-outline-light"
                        }`
                    }
                    onClick={() => setActive(item)}
                >
                    <i className={`bi ${icons[item]}`}></i>

                    {" "}

                    {item}

                </button>

            ))}

            <div className="mt-auto">

                <button
                    className="btn btn-danger w-100"
                    onClick={logout}
                >
                    <i className="bi bi-box-arrow-right"></i>

                    {" "}

                    Logout
                </button>

            </div>

        </div>
    );
}