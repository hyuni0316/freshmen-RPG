using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private PlayerUnit player;
    [SerializeField] private EnemyHud enemyHud;
    [SerializeField] private EnemyUnit enemyUnit;
    [SerializeField] private CurrentSituation _currentSituation;
    
    [SerializeField] private BattleDialogBox dialogBox;
    [SerializeField] private GameObject blocker;
    [SerializeField] private GameObject runawayUI;
    [SerializeField] private GameObject gameClearUI;
    [SerializeField] private GameObject gameOverUI;
    
    private BattleState state;
    private bool isMonsterDefeated = false;
    private bool isButtonClicked = false;
    private string playerChoice;
    private bool isGameOver = false;
    private int leftSDamageTurn = 0;
    private bool isArmorActive = false;
    
    // quiz
    [SerializeField] private GameObject quizUI;
    [SerializeField] private TextMeshProUGUI quizText;
    private int OXquizNum = -1;
    private int quizAnswer = -1;
    [SerializeField] private string[] _quizArr = new string[3];
    [SerializeField] private int[] _quizAnswerArr = new int[3];
    
    void Start()
    {
        StartCoroutine(SetupBattle()); 
        StartBattle();
    }

    void StartBattle()
    {
        StartCoroutine(coFunc_RollTurns());
    }

    IEnumerator coFunc_RollTurns()
    {
        for (int round = 0;; round++)
        {
            // 플레이어 턴
            yield return new WaitUntil(() => state == BattleState.PlayerTurn);
            yield return PlayerTurn();
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => isButtonClicked == true);
            yield return AfterPlayerTurn(playerChoice);
            yield return new WaitForSeconds(1f);
            if (isMonsterDefeated)
            {
                yield return GameClear();
                yield break;
            }
            
            // 적 몬스터 턴
            yield return EnemyTurn();
            yield return new WaitForSeconds(1f);
            AfterEnemyTurn();
            if (isGameOver)
            {
                yield return GameOver();
                yield break;
            }
        }
    }

    public IEnumerator SetupBattle()
    {
        state = BattleState.Start;
        Debug.Log("SetupBattle");
        blocker.SetActive(true);
        player.Setup();
        enemyUnit.SetUp();
        enemyHud.SetData(enemyUnit._Monster);

        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}가 나타났습니다.\n적을 물리쳐 학교의 평화를 지켜주세요.");
        yield return new WaitForSeconds(0.5f);
        state = BattleState.PlayerTurn;
    }

    IEnumerator PlayerTurn()
    {
        Debug.Log("PlayerTurn");
        state = BattleState.PlayerTurn;
        yield return dialogBox.TypeDialog("무엇을 할까요?");
        blocker.SetActive(false);
    }

    public void ButtonClicked(string selection)
    {
        Debug.Log("ButtonClicked");
        if (state != BattleState.PlayerTurn) return;
        
        playerChoice = selection;
        isButtonClicked = true;
    }
    
    IEnumerator AfterPlayerTurn(string selection)
    {
        Debug.Log("AfterPlayerTurn");
        state = BattleState.Busy;
        switch (selection)
        {
            case "first skill":
                yield return FirstSkill();
                break;
            case "second skill":
                yield return SecondSkill();
                break;
            case "third skill":
                yield return ThirdSkill();
                break;
            case "fourth skill":
                yield return FourthSkill();
                break;
            case "runaway":
                yield return RunAway();
                break;
        }
        yield return enemyHud.UpdateHP();
        if (leftSDamageTurn != 0)
        {
            leftSDamageTurn--;
            yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}가 교수님 성대모사의 여파로 인한 데미지를 받습니다.");
            int tmpNum = enemyUnit._Monster.TakeDamage(10, 20);
            if (OXquizNum == -1) OXquizNum = tmpNum;
            yield return enemyHud.UpdateHP();
            yield return dialogBox.TypeDialog(
                $"{leftSDamageTurn}턴 동안 {enemyUnit._Monster._monsterName}에게 추가 데미지를 줍니다.");
        }
        if (OXquizNum != -1)
        {
            switch (OXquizNum)
            {
                case 2:
                    yield return LastOXQuiz();
                    break;
                default:
                    yield return OXQuiz();
                    break;
            }
        }
        state = BattleState.EnemyTurn;
        isButtonClicked = false;
    }

    IEnumerator OXQuiz()
    {
        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}의 저항이 거세집니다.");
        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}의 저항을 잠재우기 위해서는 퀴즈를 맞혀 기선을 제압해야합니다.");

        quizText.text = _quizArr[OXquizNum];
        quizUI.SetActive(true);
        
        yield return new WaitUntil(() => quizAnswer != -1);

        if (quizAnswer == _quizAnswerArr[OXquizNum])
        {
            yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}를 진정시켰습니다.\n공격력이 올라가지 않습니다.");
        }
        else
        {
            yield return dialogBox.TypeDialog(
                $"{enemyUnit._Monster._monsterName}를 진정시키지 못했습니다. {enemyUnit._Monster._monsterName}의 공격력이 5 올라갑니다.");
            enemyUnit._Monster.Attack += 5;
        }
        OXquizNum = -1;
        quizAnswer = -1;
        quizUI.SetActive(false);
    }
    
    IEnumerator LastOXQuiz()
    {
        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}가 마지막 저항을 합니다..");
        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}를 쓰러뜨리기 위해 퀴즈를 맞혀 전의를 상실시키세요.");
        yield return dialogBox.TypeDialog($"퀴즈를 맞히지 못할 경우, {enemyUnit._Monster._monsterName}가 다시 체력을 회복하게 됩니다.");
        quizText.text = _quizArr[OXquizNum];
        quizUI.SetActive(true);
        
        yield return new WaitUntil(() => quizAnswer != -1);

        if (quizAnswer == _quizAnswerArr[OXquizNum])
        {
            yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}를 쓰러트렸습니다!");
            CheckGameOver();
        }
        else
        {
            yield return dialogBox.TypeDialog(
                $"{enemyUnit._Monster._monsterName}를 진정시키지 못했습니다.\n{enemyUnit._Monster._monsterName}가 체력을 회복합니다.");

            enemyUnit._Monster.TakeDamage(10, -50);
            yield return enemyHud.UpdateHP();
            yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}의 공격력이 5 올라갑니다.");
            enemyUnit._Monster.Attack += 5;
        }
        OXquizNum = -1;
        quizAnswer = -1;
        quizUI.SetActive(false);
    }

    public void QuizAnser_Onclicked(int response)
    {
        quizAnswer = response;
    }
    
    // 일반 공격
    // 과제 던지기
    IEnumerator FirstSkill()
    {
        Debug.Log("First Skill");
        int skillDamage = 30;
        yield return dialogBox.TypeDialog($"{player._playerName}의 과제 투척 공격!");
        yield return new WaitForSeconds(0.5f);
        int realDamage = enemyUnit._Monster.HP;
        OXquizNum = enemyUnit._Monster.TakeDamage(player.Attack, skillDamage);
        realDamage -= enemyUnit._Monster.HP;
        
        StartCoroutine(DamagedSprite(enemyUnit.gameObject));
        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}은 {realDamage}의 데미지를 받았습니다.");
    }

    // 도트뎀 공격
    // 교수님 성대모사
    IEnumerator SecondSkill()
    {
        yield return dialogBox.TypeDialog($"{player._playerName}의 교수님 성대모사 공격!");
        leftSDamageTurn = 4;
        yield return dialogBox.TypeDialog($"{leftSDamageTurn}턴 동안 {enemyUnit._Monster._monsterName}에게 추가 데미지를 줍니다.");
    }

    //
    IEnumerator ThirdSkill()
    {
        yield return dialogBox.TypeDialog($"{player._playerName}의 과잠 갑옷! 다음 공격을 한 번 막아줍니다.");
        isArmorActive = true;
    }

    // 유틸 공격: 랜덤으로 적공격을 낮추거나 적 방어력을 낮춘다. 랜덤으로 내 공격력을 올리거나 내 방어력을 올린다.
    // 지피티의 가호
    IEnumerator FourthSkill()
    {
        Debug.Log("Fourth Skill");
        int deltaAbil = 3;
        int random = Random.Range(0, 2);
        yield return dialogBox.TypeDialog($"{player._playerName}의 지피티의 가호!");
        switch (random)
        {
            // 0일 경우 내 공격력을 올리고 적 공격력을 낮춘다.
            case 0:
                enemyUnit._Monster.Attack -= 5;
                player.Attack += 5;
                yield return dialogBox.TypeDialog($"{player._playerName}의 공격력이 증가했습니다.");
                yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}의 공격력이 감소했습니다.");
                break;
            // 1일 경우 내 방어력을 올리고 적 방어력을 낮춘다.
            case 1:
                enemyUnit._Monster.Defense -= 5;
                player.Defense += 5;
                yield return dialogBox.TypeDialog($"{player._playerName}의 방어력이 증가했습니다.");
                yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}의 방어력이 감소했습니다.");
                break;
        }
    }

    public IEnumerator RunAway()
    {
        Debug.Log("Run away");
        dialogBox.ResetDialog();
        yield return dialogBox.TypeDialog("무사히 도망쳤습니다.");
        SceneManager.LoadScene("Scenes/MainScene");
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        state = BattleState.EnemyTurn;
        
        int random = Random.Range(0, 3);
        switch (random)
        {
            // 일반 공격
            case 0:
                yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}이 공격합니다!");
                yield return EnemyAttack(20);
                break;
            // 연속 공격
            case 1:
                yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}이 연속으로 공격합니다!");
                random = Random.Range(2, 6);
                for (int i = 0; i < random; i++)
                {
                    yield return EnemyAttack(7);
                }
                yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}이 {random}번 공격했습니다.");
                break;
            // 몬스터 유틸 스킬
            case 2:
                yield return EnemyUtilSkill();
                break;
        }
    }

    IEnumerator EnemyUtilSkill()
    {
        int deltaAbil = 3;
        int random = Random.Range(0, 2);
        switch (random)
        {
            // 공격력 증가
            case 0:
                yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}가 기합을 내지릅니다!\n{enemyUnit._Monster._monsterName}의 공격력이 증가합니다.");
                enemyUnit._Monster.Attack += deltaAbil;
                break;
            case 1:
                yield return dialogBox.TypeDialog(
                    $"{enemyUnit._Monster._monsterName}가 몸을 웅크립니다!\n{enemyUnit._Monster._monsterName}의 방어력이 증가합니다.");
                enemyUnit._Monster.Defense += deltaAbil;
                break;
        }
    }

    IEnumerator EnemyAttack(int damage)
    {
        if (isArmorActive)
        {
            yield return dialogBox.TypeDialog($"하지만 {player._playerName}의 갑옷에 막혀 데미지가 들어가지 않았습니다.");
            isArmorActive = false;
            yield break;
        }
        int realDamage = player.HP;
        isGameOver = player.TakeDamage(enemyUnit._Monster.Attack, damage);
        realDamage -= player.HP;
        yield return dialogBox.TypeDialog($"{player._playerName}는 {realDamage}의 데미지를 받았습니다.");
        player.TakeDamageEffect(enemyUnit);
        StartCoroutine(DamagedSprite(player.gameObject));
        yield return player.UpdateHP();
    }

    public void AfterEnemyTurn()
    {
        Debug.Log("AfterEnemyTurn");
        state = BattleState.PlayerTurn;
    }

    public void TryRunaway_Onclicked()
    {
        runawayUI.SetActive(true);
    }
    
    IEnumerator GameClear()
    {
        Debug.Log("GameClear");
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "HakmoonBattleScene":
                _currentSituation.HakmoonBattle = true;
                break;
            case "PoscoBattleScene":
                _currentSituation.PoscoBattle = true;
                break;
            case "AsanBattleScene":
                _currentSituation.AsanBattle = true;
                break;
        }
        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}을 무찔렀습니다!");
        gameClearUI.SetActive(true);
    }

    public void CheckGameOver()
    {
        if (enemyUnit._Monster.HP <= 0)
        {
            isMonsterDefeated = true;
        }
    }

    IEnumerator GameOver()
    {
        Debug.Log("Gameover");
        yield return dialogBox.TypeDialog("새로니는 적의 공격에 쓰러지고 말았습니다...");
        gameOverUI.SetActive(true);
    }
    
    IEnumerator DamagedSprite(GameObject go)
    {
        Image targetImage = go.GetComponent<Image>();
        for (int i = 0; i < 6; i++)
        {
            targetImage.color = new Color32(238, 169, 169, 255);
            yield return new WaitForSeconds(0.1f);
            targetImage.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
