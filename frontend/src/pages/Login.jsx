import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../api/axios";

export default function Login() {

    const navigate = useNavigate();

    const [username, setUsername] = useState("");

    const [password, setPassword] = useState("");

    async function login(e) {

        e.preventDefault();

        try {

            const response = await api.post("/auth/login", {

                username,
                password

            });

            localStorage.setItem("token", response.data.token);

            localStorage.setItem("role", response.data.role);

            localStorage.setItem("username", username);

            if (response.data.role === "Admin")
                navigate("/admin");
            else
                navigate("/student");

        }
        catch {

            alert("Invalid username or password.");

        }

    }

    return (

        <div
            className="container d-flex justify-content-center align-items-center"
            style={{ minHeight: "100vh" }}
        >

            <div
                className="card shadow p-4"
                style={{ width: "420px" }}
            >

                <h2 className="text-center mb-4">

                    📚 Library Management System

                </h2>

                <form onSubmit={login}>

                    <div className="mb-3">

                        <label className="form-label">

                            Username

                        </label>

                        <input
                            className="form-control"
                            value={username}
                            onChange={(e)=>setUsername(e.target.value)}
                        />

                    </div>

                    <div className="mb-3">

                        <label className="form-label">

                            Password

                        </label>

                        <input
                            className="form-control"
                            type="password"
                            value={password}
                            onChange={(e)=>setPassword(e.target.value)}
                        />

                    </div>

                    <button
                        className="btn btn-primary w-100"
                        type="submit"
                    >
                        Login
                    </button>

                </form>

                <hr/>

                <Link
                    className="btn btn-outline-success mb-2"
                    to="/register/student"
                >
                    Register Student
                </Link>

                <Link
                    className="btn btn-outline-dark"
                    to="/register/admin"
                >
                    Register Admin
                </Link>

            </div>

        </div>

    );

}