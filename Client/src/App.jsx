import React, { useState } from "react";
import "./App.css";

export default function App() {
	const [file, setFile] = useState(null);
	const [data, setData] = useState([]);

	const handleFileChange = (e) => {
		setFile(e.target.files[0]);
	};

	const handleUpload = async () => {
		if (!file) {
			alert("Please select a file first!");
			return;
		}

		const formData = new FormData();
		formData.append("file", file);

		try {
			const response = await fetch("/api/projects/analyze", {
				method: "POST",
				body: formData,
			});

			if (!response.ok) {
				throw new Error(`API error: ${response.statusText}`);
			}

			const result = await response.json();
			setData(result);
		} catch (error) {
			console.error("Upload failed:", error);
			alert("Error sending file to server");
		}
	};

	return (
		<div style={{ padding: "20px", fontFamily: "Arial" }}>
			<h2>Project CSV Analyzer</h2>

			<input type="file" accept=".csv" onChange={handleFileChange} style={{ marginRight: "10px" }} />
			<button onClick={handleUpload}>Analyze</button>

			<hr />

			{data.length > 0 && (
				<table border="1" cellPadding="8" style={{ borderCollapse: "collapse", width: "100%" }}>
					<thead>
						<tr>
							<th>ProjectID</th>
							<th>Employee1</th>
							<th>Employee2</th>
							<th>SumOfDaysForTopTwo</th>
						</tr>
					</thead>
					<tbody>
						{data.map((item, idx) => (
							<tr key={idx}>
								<td>{item.projectID}</td>
								<td>{item.employee1}</td>
								<td>{item.employee2}</td>
								<td>{item.sumOfDaysForTopTwo}</td>
							</tr>
						))}
					</tbody>
				</table>
			)}
		</div>
	);
}