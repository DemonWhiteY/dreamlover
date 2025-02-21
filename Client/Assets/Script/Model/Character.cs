using SQLite4Unity3d;

public class Character 
{
    [PrimaryKey, AutoIncrement] //设置主键 自动增长
    public int id { get; set; }//Id作为主键
    public string name { get; set; }

    public int modelnum { get; set; }
    public string Description { get; set; }
    public int age { get; set; }

    public float disposition { get; set; }
    public float mbti_e_i { get; set; }

    public float mbti_f_n { get; set; }

    public float mbti_f_t { get; set; }

    public float mbti_p_j { get; set; }

    public int voice { get; set; }



    /// <summary>
    /// 重写ToString函数，方便控制台打印
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("[Person: Id={0}, Name={1}, modelnum={2}, Describtion={3}],age={4},disposition={5}", id, name, modelnum, Description,age,disposition);
    }
}
