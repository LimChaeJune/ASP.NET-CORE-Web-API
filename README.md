ASP.NET CORE로 API 제작하기
======
## 개요
  ASP.NET Core로 웹 API를 빌드하는 과정을 설명합니다.    
## 시작하기
   * ASP.NET 및 웹 개발 포함된 Visual Studio 2019 16.8 이상     
   * .NET 5.0 SDK 이상 

   1. ASP.NET CORE 프로젝트 생성
   2. Model로 사용할 Class와 EntityFrameWork DataBase Context 추가
   3. CRUD를 위한 메서드로 스캐폴드 컨트롤러 생성
   4. 라우팅, URL 경로 및 메서드 반환 값 구성
   5. PostMan을 이용한 API 호출 테스트

  
  ### 1. ASP.NET CORE 프로젝트 생성
  - 비주얼 스튜디오에서 **새로 만들기 > 새 프로젝트 > ASP.NET CORE 웹 애플리케이션**  
  - AspCoreApi로 프로젝트 이름 설정 뒤 **API** 항목을 선택해 프로젝트 생성
  ![1](https://github.com/LimChaeJune/ASP.NET-CORE-Web-API/blob/master/1.png)
  ![2](https://github.com/LimChaeJune/ASP.NET-CORE-Web-API/blob/master/2.png)
  ![3](https://github.com/LimChaeJune/ASP.NET-CORE-Web-API/blob/master/3.png)
    #### 1.1 launchUrl 업데이트    
    launchUrl의 항목을 사용할 api 주소로 변경해줍니다.  
    
    `Properties/launchSetting.json`   
    
    ```json
    "launchUrl": "api/TodoItems",
    ``` 
   ---
  ### 2. Model Class 추가
  모델은 앱에서 관리할 데이터를 나타내는 클래스의 집합입니다.   
  - 프로젝트에서 **추가 > 새폴더** 폴더 이름은 Model로 생성  
  - Model 폴더에서 **추가 > 클래스** 클래스 이름 TodoItem으로 생성 
  
`TodoItem.cs` 
```csharp
namespace AspCoreApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
``` 
- id는 Primary Key 역할을 합니다.
  
  ---
  ### 3. DataBase Context 추가
  DataBase Context는 데이터 모델에 대한 엔티티 프레임 워크의 기능을 조정하는 메인 클래스입니다.  
  #### Nuget 패키지 추가
    - 솔루션에서 오른쪽 클릭으로 Nuget 패키지 관리자 진입
    - 찾아보기 탭을 누른다음 Microsoft.EntityFrameworkCore.SqlServer 입력후 해당 Nuget을 선택 후 설치
  
  - Models 폴더에서 **추가 > 클래스** 클래스 이름 ApiDBContext 생성
    
`ApiDBContext.cs`
```csharp
using Microsoft.EntityFrameworkCore;

namespace AspCoreApi.Models
{
    public class ApiDBContext : DbContext
    { 
        public ApiDBContext(DbContextOptions<ApiDBContext> options) : base(options)
        {

        }
        public DbSet<TodoItem> TodoItem { get; set; }
    }
}
```

---
  ### 4. DataBase Context 등록
  ASP.NET Core에서 DB Context와 같은 서비스는 [DI(종속성 주입)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0) 컨테이너에 등록 되어야합니다.
  
`StartUp.cs`

```csharp
using AspCoreApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;


namespace AspCoreApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiDBContext>(opt =>
                                   opt.UseInMemoryDatabase("TodoList"));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```

---
  ### 5. 스캐 폴드 컨트롤러 추가
  - Controllers 폴더에서 **추가 > 스캐폴드 항목 새로 만들기(F)** 선택
  - Entity Framework를 사용하며 동작이 포함된 API 컨트롤러 추가
  - Model과 해당 Model의 Database Context를 선택 해줍니다. (예제는 아래와 같습니다.) 
    * **모델 클래스**에서 TodoItem (AspCoreApi.Models) 를 선택합니다.
    * **데이터 컨텍스트 클래스**에서 ApiDBContext (AspCoreApi.Models) 를 선택합니다.
  아래와 같이 Controller 코드를 수정합니다.
`TodoItemsController`
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspCoreApi.Models;

namespace AspCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ApiDBContext _context;

        public TodoItemsController(ApiDBContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItem()
        {
            return await _context.TodoItem.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItem.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItem.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItem.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItem.Any(e => e.Id == id);
        }
    }
}
```
* 위의 코드 내용은 아래와 같습니다.  

|API|기술|요청 본문|응답 본문|
|------|---|---|---|  
|GET /api/|모든 할 일 항목 가져 오기|없음|할 일 항목 배열|  
|GET /api/TodoItems/{id}|ID로 항목 받기|없음|할 일 항목| 
|POST /api/TodoItems|새 항목 추가|할 일 항목|할 일 항목| 
|PUT /api/TodoItems/{id}|기존 항목 업데이트|할 일 항목|없음|  
|DELETE /api/TodoItems/{id}|항목 삭제|없음|없음|  

---
### 6. Postman을 이용한 테스트
[PostMan 설치](https://www.postman.com/downloads/)
PostMan을 사용하여 위에 생성한 웹 API를 테스트합니다.  

  #### 6.1 Post TodoItem
  - HTTP 메서드를 POST로 변경  
  - URI 설정 http://localhost:포트번호/api/TodoItems (포트 번호는 프로젝트의 Properties/launchSetting.json 파일의 iisSettings속성에서 확인할 수 있습니다.)
  - **Body** 탭 선택 
  - **raw** 라디오 버튼 선택
  - **JSON**으로 유형 변경 
  Body의 아래와 같이 입력 
  ```json
  {
    "id": 1,
    "name": "API 테스트",
    "isComplete": true
  }
  ``` 
  - **Send** 클릭 
  
  위에 단계를 수행하면 아래와 같은 결과를 보여줍니다. 
  ![4](https://github.com/LimChaeJune/ASP.NET-CORE-Web-API/blob/master/images/1.png)
  
  
  
  
  
  
