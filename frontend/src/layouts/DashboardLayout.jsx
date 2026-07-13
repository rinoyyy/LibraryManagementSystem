import Sidebar from "../components/layout/Sidebar";
import Navbar from "../components/layout/Navbar";

export default function DashboardLayout({
    title,
    username,
    menuItems,
    activeItem,
    setActiveItem,
    children
}) {

    function logout() {

        localStorage.clear();

        window.location.href = "/";

    }

    return (

        <div className="dashboard">

            <Sidebar
                items={menuItems}
                active={activeItem}
                setActive={setActiveItem}
                logout={logout}
            />

            <div className="content">

                <Navbar
                    title={title}
                    username={username}
                />

                {children}

            </div>

        </div>

    );

}