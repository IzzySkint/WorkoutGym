import IAdminService = WorkoutGym.IAdminService;
import {AdminService} from "services/AdminService";

export class ViewSessionsView{
    
    private service: IAdminService;
    private $container: JQuery;
    private $memberSessionsFilter: JQuery;
    private $downloadTimeTable: JQuery;
    private dataTable: DataTables.Api;
    
    constructor(service: IAdminService) {
        
        this.service = service;
        this.$container = $.dataJs("view-sessions");
        
        this.init();
        this.registerEvents();
        this.loadMemberSessions();
    }
    
    private init(): void{
        
        this.$memberSessionsFilter = this.$container.find("#memberSessionsFilter");
        this.$downloadTimeTable = this.$container.find("#downloadTimeTable");
        this.$downloadTimeTable.hide();
        this.dataTable = this.$container.find("#memberSessions").DataTable({
            pageLength: 10,
            columnDefs: [
                {targets: 0, visible: false}
            ],
            columns: [
                {data: "sessionId"},
                {data: "date"},
                {data: "member"},
                {data: "email"},
                {data: "area"},
                {data: "startTime"},
                {data: "endTime"}
            ]
        });
    }
    
    private registerEvents(): void{
        this.$memberSessionsFilter.on("change", () => {
            this.loadMemberSessions();
        });
    }

    private loadMemberSessions(): void {

        let filter = this.$memberSessionsFilter.val();
        this.dataTable.clear();

        if (filter === "1"){
            this.$downloadTimeTable.show();
            
            this.service.getMemberSessionsByDate($.currentDate())
                .then((data) =>{
                    this.dataTable.rows.add(data);
                    this.dataTable.draw();
                })
                .catch((error) => {
                    console.log(error);
                });
        }
        else if (filter === "2"){
            this.$downloadTimeTable.hide();
            
            this.service.getMemberSessionsByDateRange($.startCurrentMonthDate(), $.endCurrentMonthDate())
                .then((data) => {
                    this.dataTable.rows.add(data);
                    this.dataTable.draw();
                }).catch((error) => {
                console.log(error);
            });
        }
    }
}

var viewSessionsView = new ViewSessionsView(new AdminService());