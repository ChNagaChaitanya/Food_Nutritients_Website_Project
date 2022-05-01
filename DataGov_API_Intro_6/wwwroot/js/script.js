// Script for side navigation
function w3_open() {
	var x = document.getElementById("mySidebar");
	x.style.width = "300px";
	x.style.paddingTop = "10%";
	x.style.display = "block";
}
// Close side navigation
function w3_close() {
	document.getElementById("mySidebar").style.display = "none";
}
// Used to toggle the menu on smaller screens when clicking on the menu button
function openNav() {
	var x = document.getElementById("navDemo");
	if(x.className.indexOf("show") == -1) {
		x.className += " show";
	} else {
		x.className = x.className.replace(" show", "");
	}
}

function myFunction() {
	// Declare variables
	var input, filter, table, tr, td, i, txtValue;
	input = document.getElementById("myInput");
	filter = input.value.toUpperCase();
	table = document.getElementById("myTable");
	tr = table.getElementsByTagName("tr");
	// Loop through all table rows, and hide those who don't match the search query
	for (i = 0; i < tr.length; i++) {
		td = tr[i].getElementsByTagName("td")[0];
		if (td) {
			txtValue = td.textContent || td.innerText;
			if (txtValue.toUpperCase().indexOf(filter) > -1) {
				tr[i].style.display = "";
			} else {
				tr[i].style.display = "none";
			}
		}
	}
}

function createformvalidation() {
	
	var x, x1, x2, x3, x4, x5, text, text1;
	var l = [];
	// Get the value of the input field with id="numb"
	x1 = document.getElementById("prot").value;
	x2 = document.getElementById("carb").value;
	x3 = document.getElementById("fat").value;
	x4 = document.getElementById("sug").value;
	x5 = document.getElementById("eng").value;
	l.push(x1);
	l.push(x2);
	l.push(x3);
	l.push(x4);
	l.push(x5);
	// If x is Not a Number or less than one or greater than 10
	for (let x in l) {
		if (isNaN(x) || x < 0 || x > 1000000 || !(x1) || !(x2) || !(x3) || !(x)) {
			text = "Record not Created";
			text1 = "Please provide input for all fields and make sure the Value field is numeric";
			break;
		} else {
			text = "Record Created Successfully";
			text1 = ""
		}
	}
	document.getElementById("demo").innerHTML = text;
	document.getElementById("demo1").innerHTML = text1;

}

function deleteformvalidation() {
	var x, text, text1;
	var res = confirm("Are you sure you want to delete the record?")
	if (res) {
		text = "Record Deleted Successfully";
		text1 = "";
	} else {
		text = "Record not Deleted";
		text1 = "Please select Yes to delete.";
	}
	document.getElementById("demo").innerHTML = text;
	document.getElementById("demo1").innerHTML = text1;
}

function updateformvalidation() {
	var x, x1, x2, x3, x4, x5, text, text1;
	var l = [];
	// Get the value of the input field with id="numb"
	x1 = document.getElementById("prot").value;
	x2 = document.getElementById("carb").value;
	x3 = document.getElementById("fat").value;
	x4 = document.getElementById("sug").value;
	x5 = document.getElementById("eng").value;
	l.push(x1);
	l.push(x2);
	l.push(x3);
	l.push(x4);
	l.push(x5);

	// If x is Not a Number or less than one or greater than 10
	for (let x in l) {
		if (isNaN(x) || x < 0 || x > 1000000 || !(x)) {
			text = "Record not Updated";
			text1 = "Please make sure the Value field is numeric";
			break;
		} else {
			text = "Record Updated Successfully";
			text1 = ""
		}
	}
	document.getElementById("demo").innerHTML = text;
	document.getElementById("demo1").innerHTML = text1;

}


function togetvalue(ele) {
	var food, prot, carb, fat, sug, eng;
	localStorage.setItem("food", ele.parentNode.parentNode.children[0].innerHTML);
	localStorage.setItem("prot", ele.parentNode.parentNode.children[1].innerHTML);
	localStorage.setItem("carb", ele.parentNode.parentNode.children[2].innerHTML);
	localStorage.setItem("fat", ele.parentNode.parentNode.children[3].innerHTML);
	localStorage.setItem("sug", ele.parentNode.parentNode.children[4].innerHTML);
	localStorage.setItem("eng", ele.parentNode.parentNode.children[5].innerHTML);
}

function todeletevalue(ele) {
	var food, prot, carb, fat, sug, eng;
	localStorage.setItem("food", ele.parentNode.parentNode.children[0].innerHTML);
	localStorage.setItem("prot", ele.parentNode.parentNode.children[1].innerHTML);
	localStorage.setItem("carb", ele.parentNode.parentNode.children[2].innerHTML);
	localStorage.setItem("fat", ele.parentNode.parentNode.children[3].innerHTML);
	localStorage.setItem("sug", ele.parentNode.parentNode.children[4].innerHTML);
	localStorage.setItem("eng", ele.parentNode.parentNode.children[5].innerHTML);
}


//chart js --- chart for analyze tab
var dataObjects = [{
	food: document.getElementById("demo2").innerHTML,
	label: "Protein(G)",
	data: document.getElementById("demo3").innerHTML
}, {
		food: document.getElementById("demo2").innerHTML,
	label: "Carbohydrate(G)",
	data: document.getElementById("demo4").innerHTML
	}, {
		food: document.getElementById("demo2").innerHTML,
	label: "Sugars(G)",
	data: document.getElementById("demo5").innerHTML
}]
/* data */
var data = {
	labels: dataObjects[0].food,
	datasets: [{
		label: dataObjects[0].label,
		data: dataObjects[0].data,
		/* global setting */
		backgroundColor: 'rgba(54, 162, 235, 0.2)',
		borderColor: 'rgba(54, 162, 235, 1)',
		borderWidth: 1,
	}]
};
var options = {
	legend: {
		display: true,
		fillStyle: "red",
		labels: {
			boxWidth: 0,
			fontSize: 24,
			fontColor: "black",
			fontStyle: "bold",
		},
		responsive: true,
		maintainAspectRatio: false,
	},
	scales: {
		xAxes: [{
			ticks: {
				callback: function (food) {
					if (/\s/.test(food)) {
						return food.split(" ");
					} else {
						return food;
					}
				}
			}
		}],
		yAxes: [{
			stacked: true,
			scaleLabel: {
				display: true,
				labelString: 'Grams'
			}
		}]
	},
	/*end scales */
	plugins: {
		datalabels: {
			color: 'black',
			font: {
				size: 25,
				style: "bold",
			}
		}
	}
};
var chart = new Chart('chart-0', {
	plugins: [ChartDataLabels],
	type: 'bar',
	data: data,
	options: options
});

function changeData(index) {
	chart.data.datasets.forEach(function (dataset) {
		data.labels = dataObjects[index].food;
		dataset.label = dataObjects[index].label;
		dataset.data = dataObjects[index].data;
		//dataset.backgroundColor = dataObjects[index].backgroundColor;
	});
	chart.update();
}
/* add active class on click */
// Add active class to the current button (highlight it)
var header = document.getElementById("myDIV");
var btns = header.getElementsByClassName("btn");
for (var i = 0; i < btns.length; i++) {
	btns[i].addEventListener("click", function () {
		var current = document.getElementsByClassName("active");
		current[0].className = current[0].className.replace(" active", "");
		this.className += " active";
	});
}