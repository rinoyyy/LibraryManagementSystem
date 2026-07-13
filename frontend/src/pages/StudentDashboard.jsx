import { useState } from "react";

import DashboardLayout from "../layouts/DashboardLayout";

export default function StudentDashboard() {

    const [activeItem, setActiveItem] = useState("Dashboard");

    function renderContent() {

        switch (activeItem) {

            case "Dashboard":

                return <h2>Dashboard</h2>;

            case "Available Books":

                return <h2>Available Books</h2>;

            case "My Books":

                return <h2>My Books</h2>;

            case "Borrow History":

                return <h2>Borrow History</h2>;

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