import { makeGetRequest, sendPost } from './api-service';


class FakeServer {

	vehicles = [{ Id: 1, Name: "Opel", Kilometer: 50000 },
	{ id: 2, name: "BMW", kilometer: 33000 }]

	actionTemplates = { 1: [{ id: 1, name: "Oil exchange", KilometerInterval: 5000 }, 
	{ id: 2, name: "Lights", KilometerInterval: 50000 }] }

	public async getVehicles() {
		return this.vehicles;
	}

	public async getActionTemplates(vehicleId) {
		//return this.actionTemplates[vehicleId];
	}

	public async getVehicleSummary(vehicleId){
		let relevant = this.vehicles.filter(_ => _.Id == vehicleId)[0]
		return relevant;
	}

	public async getActions(vehicleId) {

	}

	public async createVehicle(name, kilometer) {
		
	}

	public async addActionTemplate(vehicleId, name, kilometerInterval, timeInterval) {
		
	}

	public async addAction(vehicleId, actionTemplateId, date, kilometer, note) {

	}
}


export default FakeServer;
