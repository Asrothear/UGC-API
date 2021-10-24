using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Database_Models;

namespace UGC_API.Functions
{
    public class Systems
    {
        public static List<DB_ambiabar> _System_ambiabar = new List<DB_ambiabar>();
        public static List<DB_anahit> _System_anahit = new List<DB_anahit>();
        public static List<DB_angurongo> _System_angurongo = new List<DB_angurongo>();
        public static List<DB_bpm_16204> _System_bpm_16204 = new List<DB_bpm_16204>();
        public static List<DB_cernunnos> _System_cernunnos = new List<DB_cernunnos>();
        public static List<DB_crom_dubh> _System_crom_dubh = new List<DB_crom_dubh>();
        public static List<DB_dall> _System_dall = new List<DB_dall>();
        public static List<DB_delta_phoenicis> _System_delta_phoenicis = new List<DB_delta_phoenicis>();
        public static List<DB_duronese> _System_duronese = new List<DB_duronese>();
        public static List<DB_hip_2747> _System_hip_2747 = new List<DB_hip_2747>();
        public static List<DB_hip_3603> _System_hip_3603 = new List<DB_hip_3603>();
        public static List<DB_hip_4764> _System_hip_4764 = new List<DB_hip_4764>();
        public static List<DB_hip_4964> _System_hip_4964 = new List<DB_hip_4964>();
        public static List<DB_hip_5099> _System_hip_5099 = new List<DB_hip_5099>();
        public static List<DB_hyperborea> _System_hyperborea = new List<DB_hyperborea>();
        public static List<DB_kartamayana> _System_kartamayana = new List<DB_kartamayana>();
        public static List<DB_khampti> _System_khampti = new List<DB_khampti>();
        public static List<DB_kharpulo> _System_kharpulo = new List<DB_kharpulo>();
        public static List<DB_kunggalerni> _System_kunggalerni = new List<DB_kunggalerni>();
        public static List<DB_liu_di> _System_liu_di = new List<DB_liu_di>();
        public static List<DB_ltt_518> _System_ltt_518 = new List<DB_ltt_518>();
        public static List<DB_ltt_874> _System_ltt_874 = new List<DB_ltt_874>();
        public static List<DB_maidubrigel> _System_maidubrigel = new List<DB_maidubrigel>();
        public static List<DB_minanes> _System_minanes = new List<DB_minanes>();
        public static List<DB_nayanezgani> _System_nayanezgani = new List<DB_nayanezgani>();
        public static List<DB_niflhel> _System_niflhel = new List<DB_niflhel>();
        public static List<DB_nltt_2682> _System_nltt_2682 = new List<DB_nltt_2682>();
        public static List<DB_paras> _System_paras = new List<DB_paras>();
        public static List<DB_piperish> _System_piperish = new List<DB_piperish>();
        public static List<DB_rosmerta> _System_rosmerta = new List<DB_rosmerta>();
        public static List<DB_runo> _System_runo = new List<DB_runo>();
        public static List<DB_sadhbh> _System_sadhbh = new List<DB_sadhbh>();
        public static List<DB_slatas> _System_slatas = new List<DB_slatas>();
        public static List<DB_tetela> _System_tetela = new List<DB_tetela>();
        public static List<DB_tocorii> _System_tocorii = new List<DB_tocorii>();
        public static List<DB_wapiya> _System_wapiya = new List<DB_wapiya>();
        internal static void LoadFromDB(Database.DBContext db)
        {
           _System_ambiabar = new List<DB_ambiabar>(db.ambiabar);
           _System_anahit = new List<DB_anahit>(db.anahit);
           _System_angurongo = new List<DB_angurongo>(db.angurongo);
           _System_bpm_16204 = new List<DB_bpm_16204>(db.bpm_16204);
           _System_cernunnos = new List<DB_cernunnos>(db.cernunnos);
           _System_crom_dubh = new List<DB_crom_dubh>(db.crom_dubh);
           _System_dall = new List<DB_dall>(db.dall);
           _System_delta_phoenicis = new List<DB_delta_phoenicis>(db.delta_phoenicis);
           _System_duronese = new List<DB_duronese>(db.duronese);
           _System_hip_2747 = new List<DB_hip_2747>(db.hip_2747);
           _System_hip_3603 = new List<DB_hip_3603>(db.hip_3603);
           _System_hip_4764 = new List<DB_hip_4764>(db.hip_4764);
           _System_hip_4964 = new List<DB_hip_4964>(db.hip_4964);
           _System_hip_5099 = new List<DB_hip_5099>(db.hip_5099);
           _System_hyperborea = new List<DB_hyperborea>(db.hyperborea);
           _System_kartamayana = new List<DB_kartamayana>(db.kartamayana);
           _System_khampti = new List<DB_khampti>(db.khampti);
           _System_kharpulo = new List<DB_kharpulo>(db.kharpulo);
           _System_kunggalerni = new List<DB_kunggalerni>(db.kunggalerni);
           _System_liu_di = new List<DB_liu_di>(db.liu_di);
           _System_ltt_518 = new List<DB_ltt_518>(db.ltt_518);
           _System_ltt_874 = new List<DB_ltt_874>(db.ltt_874);
           _System_maidubrigel = new List<DB_maidubrigel>(db.maidubrigel);
           _System_minanes = new List<DB_minanes>(db.minanes);
           _System_nayanezgani = new List<DB_nayanezgani>(db.nayanezgani);
           _System_niflhel = new List<DB_niflhel>(db.niflhel);
           _System_nltt_2682 = new List<DB_nltt_2682>(db.nltt_2682);
           _System_paras = new List<DB_paras>(db.paras);
           _System_piperish = new List<DB_piperish>(db.piperish);
           _System_rosmerta = new List<DB_rosmerta>(db.rosmerta);
           _System_runo = new List<DB_runo>(db.runo);
           _System_sadhbh = new List<DB_sadhbh>(db.sadhbh);
           _System_slatas = new List<DB_slatas>(db.slatas);
           _System_tetela = new List<DB_tetela>(db.tetela);
           _System_tocorii = new List<DB_tocorii>(db.tocorii);
           _System_wapiya = new List<DB_wapiya>(db.wapiya);
        }
        internal static int CountAllData()
        {
            int count = _System_ambiabar.Count +
                _System_anahit.Count +
                _System_angurongo.Count +
                _System_bpm_16204.Count +
                _System_cernunnos.Count +
                _System_crom_dubh.Count +
                _System_dall.Count +
                _System_delta_phoenicis.Count +
                _System_duronese.Count +
                _System_hip_2747.Count +
                _System_hip_3603.Count +
                _System_hip_4764.Count +
                _System_hip_4964.Count +
                _System_hip_5099.Count +
                _System_hyperborea.Count +
                _System_kartamayana.Count +
                _System_khampti.Count +
                _System_kharpulo.Count +
                _System_kunggalerni.Count +
                _System_liu_di.Count +
                _System_ltt_518.Count +
                _System_ltt_874.Count +
                _System_maidubrigel.Count +
                _System_minanes.Count +
                _System_nayanezgani.Count +
                _System_niflhel.Count +
                _System_nltt_2682.Count +
                _System_paras.Count +
                _System_piperish.Count +
                _System_rosmerta.Count +
                _System_runo.Count +
                _System_sadhbh.Count +
                _System_slatas.Count +
                _System_tetela.Count +
                _System_tocorii.Count +
                _System_wapiya.Count
                ;
            return count;
        }
    }
}