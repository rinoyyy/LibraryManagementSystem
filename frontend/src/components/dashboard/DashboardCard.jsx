export default function DashboardCard({
    title,
    value,
    icon,
    color
}) {

    return (

        <div className="col-md-3">

            <div className={`card text-white bg-${color}`}>

                <div className="card-body">

                    <h5>

                        <i className={`bi ${icon}`}></i>

                        {" "}

                        {title}

                    </h5>

                    <h2>{value}</h2>

                </div>

            </div>

        </div>

    );

}