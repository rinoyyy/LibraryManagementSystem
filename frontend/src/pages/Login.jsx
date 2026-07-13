import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import api from "../api/axios";

export default function Login() {
    const navigate = useNavigate();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    async function handleLogin(e) {
        e.preventDefault();

        try {
            const response = await api.post("/auth/login", {
                username,
                password
            });

            localStorage.setItem("token", response.data.token);
            localStorage.setItem("username", response.data.username);
            localStorage.setItem("role", response.data.role);

            if (response.data.role === "Admin") {
                navigate("/admin");
            } else {
                navigate("/student");
            }
        }
        catch {
            alert("Invalid username or password");
        }
    }

    return (
        <div
            style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                height: "100vh"
            }}
        >
            <form onSubmit={handleLogin}>
                <h1>Library Login</h1>

                <input
                    type="text"
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />

                <br /><br />

                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />

                <br /><br />

                <button type="submit">
                    Login
                </button>
                <br /><br />

<Link to="/register/student">
    Register Student
</Link>

<br /><br />

<Link to="/register/admin">
    Register Admin
</Link>
            </form>
        </div>
    );
}