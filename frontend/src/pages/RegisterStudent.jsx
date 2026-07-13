import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../api/axios";

export default function RegisterStudent() {

    const navigate = useNavigate();

    const [form, setForm] = useState({
        username: "",
        password: "",
        name: "",
        email: ""
    });

    async function register(e) {

        e.preventDefault();

        try {

            await api.post("/auth/register/student", form);

            alert("Student registered successfully.");

            navigate("/");

        }
        catch {

            alert("Registration failed.");

        }

    }

    return (

        <div style={{ padding: 40 }}>

            <h2>Register Student</h2>

            <form onSubmit={register}>

                <input
                    placeholder="Username"
                    value={form.username}
                    onChange={(e)=>setForm({...form,username:e.target.value})}
                />

                <br/><br/>

                <input
                    placeholder="Password"
                    type="password"
                    value={form.password}
                    onChange={(e)=>setForm({...form,password:e.target.value})}
                />

                <br/><br/>

                <input
                    placeholder="Full Name"
                    value={form.name}
                    onChange={(e)=>setForm({...form,name:e.target.value})}
                />

                <br/><br/>

                <input
                    placeholder="Email"
                    value={form.email}
                    onChange={(e)=>setForm({...form,email:e.target.value})}
                />

                <br/><br/>

                <button type="submit">
                    Register
                </button>

            </form>

        </div>

    );

}