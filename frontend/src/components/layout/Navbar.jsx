export default function Navbar({ title, username }) {

    return (

        <nav className="navbar navbar-expand-lg bg-white shadow-sm rounded mb-4">

            <div className="container-fluid">

                <span className="navbar-brand fw-bold">

                    📚 {title}

                </span>

                <span className="fw-semibold">

                    Welcome, {username}

                </span>

            </div>

        </nav>

    );

}