import { BrowserRouter, Routes, Route } from "react-router-dom";

import Login from "./pages/Login";
import RegisterStudent from "./pages/RegisterStudent";
import RegisterAdmin from "./pages/RegisterAdmin";
import StudentDashboard from "./pages/StudentDashboard";
import AdminDashboard from "./pages/AdminDashboard";
import ProtectedRoute from "./components/common/ProtectedRoute";

function App() {
    return (
        <BrowserRouter>
            <Routes>

                <Route path="/" element={<Login />} />

                <Route
                    path="/register/student"
                    element={<RegisterStudent />}
                />

                <Route
                    path="/register/admin"
                    element={<RegisterAdmin />}
                />

                <Route
    path="/student"
    element={
        <ProtectedRoute role="Student">
            <StudentDashboard />
        </ProtectedRoute>
    }
/>

<Route
    path="/admin"
    element={
        <ProtectedRoute role="Admin">
            <AdminDashboard />
        </ProtectedRoute>
    }
/>

            </Routes>
        </BrowserRouter>
    );
}

export default App;